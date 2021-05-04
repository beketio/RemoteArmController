using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ArmController.Core.Arm;

namespace ArmController.Core.Communication
{
    public class NetworkCommunicator : ICommunicator
    {
        private const int HeaderLength = 4;
        private const string SendStartCommand = "strt";
        private const string SendStopCommand = "stop";
        private const string SendDataCommand = "data";
        private const string SendAliveCommand = "aliv";
        private const string RecieveVersionErrorCommand = "verr";
        private const string RecieveStartCommand = "redy";
        private const string RecieveAliveCommand = "yalv";

        private static readonly byte[] SendStartBytes = Encoding.ASCII.GetBytes(SendStartCommand);
        private static readonly byte[] SendStopBytes = Encoding.ASCII.GetBytes(SendStopCommand);
        private static readonly byte[] SendDataBytes = Encoding.ASCII.GetBytes(SendDataCommand);
        private static readonly byte[] SendAliveBytes = Encoding.ASCII.GetBytes(SendAliveCommand);

        // Timers and settings
        private readonly int connectTimeout = 3000;
        private readonly Stopwatch updateTimer = new Stopwatch();
        private readonly int updateInterval = 0;
        private readonly Stopwatch checkAliveTimer = new Stopwatch();
        private readonly int checkAliveInterval = 1000; // Set to 0 to never check
        private readonly int checkAliveTimeout = 500; // Check for response within this time

        private Action<bool, string> statusListener;

        // Public vars
        private bool connected = false;

        // Network Vars
        private Socket socket;
        private IPEndPoint udpDeviceEndPoint;

        private int lastUpdateTime = 0;

        private byte packetNum = 0;

        public NetworkCommunicator()
        {
        }

        public void Connect(string remoteAddress, int port)
        {
            Disconnect();

            // this.port = port;
            IPAddress ipaddr;
            try
            {
                ipaddr = IPAddress.Parse(remoteAddress);
            }
            catch (FormatException)
            {
                try
                {
                    ipaddr = Dns.GetHostEntry(remoteAddress).AddressList[0];
                }
                catch (SocketException exp)
                {
                    throw new ComponentException(exp);
                }
            }

            udpDeviceEndPoint = new IPEndPoint(ipaddr, port);

            packetNum = 0;

            // Set up the socket
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            // Send our start message
            try
            {
                socket.SendTo(SendStartBytes, udpDeviceEndPoint);
            }
            catch (SocketException exception)
            {
                throw new ComponentException("Invalid Address", exception);
            }

            // Now let's listen for a response
            byte[] data = new byte[256];
            socket.ReceiveTimeout = connectTimeout;
            try
            {
                EndPoint remote = udpDeviceEndPoint;
                socket.ReceiveFrom(data, ref remote);
            }
            catch (SocketException exc)
            {
                Console.WriteLine("Couldn't connect");
                throw new ComponentException(exc);
            }

            // Store the response header
            string header = Encoding.ASCII.GetString(data, 0, HeaderLength);

            // Check alive
            CheckAlive();

            Console.WriteLine("Message received from {0}:", udpDeviceEndPoint.ToString());
            Console.WriteLine("Header=" + header);

            connected = true;
            statusListener.Invoke(true, "Connected");
        }

        public void Disconnect()
        {
            connected = false;

            if (socket == null)
                return;

            // Send stop message
            socket.SendTo(SendStopBytes, udpDeviceEndPoint);

            // Wait for everything to finish up
            System.Threading.Thread.Sleep(500);
            try
            {
                // Shutdown socket
                socket.Shutdown(SocketShutdown.Both);
            }
            catch (SocketException exc)
            {
                // ???
            }

            socket.Close();
            socket.Dispose();
            socket = null;
            statusListener.Invoke(false, string.Empty);
        }

        public void RegisterStatusListener(Action<bool, string> listener)
        {
            this.statusListener = listener;
        }

        public bool Update(RotationData data)
        {
            if (!connected)
                return false;

            // Use a new thread to send the packet
            System.Threading.Tasks.Task.Run(() => SendData(data));

            // Periodically check for a response
            if (!checkAliveTimer.IsRunning)
                checkAliveTimer.Start();
            if (checkAliveInterval > 0 && checkAliveTimer.ElapsedMilliseconds >= checkAliveInterval)
            {
                checkAliveTimer.Restart();
                System.Threading.Tasks.Task.Run(() => CheckAlive());
            }

            return true;
        }

        private void Disconnect(Exception exc)
        {
            Disconnect();
            statusListener.Invoke(false, exc.Message);
        }

        private void SendData(RotationData data)
        {
            if (!updateTimer.IsRunning)
                updateTimer.Start();

            // Check when the last update was
            if (updateTimer.ElapsedMilliseconds >= updateInterval)
            {
                byte[] packet = GetPacket(data);
                SendPacket(packet);

                // Packet was sent so restart the timer
                updateTimer.Restart();
            }
        }

        private byte[] GetPacket(RotationData data)
        {
            float[] rawData = data.ToArray();
            int rawDataSize = rawData.Length * 4; // Array size * 4 bytes per float

            int offset = HeaderLength + 1 + 4; // Header length + packet number (1 byte) + data size (4 bytes)
            byte[] packet = new byte[offset + rawDataSize];
            int pos = 0;

            // Write command
            for (int i = 0; i < HeaderLength; i++)
                packet[pos++] = SendDataBytes[i];

            // Write packet number
            packet[pos++] = packetNum++;

            // Write data size (2 bytes)
            packet[pos++] = Convert.ToByte(data.GetNumJoints()); // Number of joints, probably not going to be > 255
            packet[pos++] = Convert.ToByte(data.GetNumTools()); // Number of tools, probably not going to be > 255

            // Write rotation data (4 bytes per float)
            for (int i = 0; i < rawData.Length; i++)
            {
                byte[] floatbytes = BitConverter.GetBytes(rawData[i]);
                for (int j = 0; j < 4; j++)
                    packet[pos++] = floatbytes[j];
            }

            return packet;
        }

        private void SendPacket(byte[] packet)
        {
            try
            {
                socket.SendTo(packet, udpDeviceEndPoint);
                Console.WriteLine("sent!");
            }
            catch (SocketException exc)
            {
                Disconnect(exc);
            }
        }

        private bool CheckAlive(bool tryAgain = true)
        {
            byte[] buffer = new byte[HeaderLength + 1];
            int pos = 0;
            foreach (byte b in SendAliveBytes)
            {
                buffer[pos++] = b;
            }

            buffer[pos++] = packetNum;

            // Set timeout for response
            socket.ReceiveTimeout = checkAliveTimeout;

            byte[] data = new byte[256];

            // Send a request for an alive response
            socket.SendTo(buffer, udpDeviceEndPoint);
            try
            {
                // Wait for a response
                EndPoint remote = udpDeviceEndPoint;
                socket.ReceiveFrom(data, ref remote);

                // Response recieved, record time since the initial request
                lastUpdateTime = (int)checkAliveTimer.ElapsedMilliseconds;
            }
            catch (SocketException)
            {
                // Try one more time in case the packet was dropped
                if (tryAgain)
                {
                    return CheckAlive(false);
                }

                // No response on the second check
                else
                {
                    ComponentException exc = new ComponentException("Timeout!");
                    Disconnect(exc);
                    throw exc;
                }
            }

            // Store command in byte array
            string header = Encoding.ASCII.GetString(data, 0, HeaderLength);

            // Check if the response is correct
            if (header == RecieveAliveCommand)
            {
                Console.WriteLine("Got response in " + lastUpdateTime + "ms");
            }

            // Wrong response, let's try again
            else if (tryAgain)
            {
                Console.WriteLine("Header mismatch, let's try again");
                return CheckAlive(false);
            }

            // Wrong response again
            else
            {
                ComponentException exc = new ComponentException("Wrong response");
                Disconnect(exc);
                throw exc;
            }

            statusListener.Invoke(true, lastUpdateTime + "ms");

            return true;
        }
    }
}
