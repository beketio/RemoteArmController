using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmController.Core.Arm;
using ArmController.Core.Communication;
using ArmController.Core.Input;

namespace ArmController.Core
{

    public enum CommunicatorMethod
    {
        Remote,
        Serial
    }

    public enum InputMethod
    {
        Gamepad,
        SteamVR,
        LeapMotion
    }

    public sealed class Controller
    {
        private readonly RobotArm arm = new RobotArm();

        private ICommunicator communicator;
        private IInputDevice inputDevice;

        public Controller()
        {
            arm = new RobotArm();

            NetworkCommunicator = new NetworkCommunicator();

            XInput = new XInputDevice();
            SteamVr = new SteamVRDevice();
            LeapMotion = new LeapMotionDevice();

            communicator = NetworkCommunicator;
            inputDevice = XInput;

            System.Threading.Tasks.Task.Run(() => Loop());
        }

        public NetworkCommunicator NetworkCommunicator { get; set; }

        public XInputDevice XInput { get; }

        public SteamVRDevice SteamVr { get; }

        public LeapMotionDevice LeapMotion { get; }

        public void RemoteConnect(string server, int port)
        {
            NetworkCommunicator.Connect(server, port);
        }

        public void RemoteDisconnect()
        {
            NetworkCommunicator.Disconnect();
        }

        public void SetCommunicatorMethod(CommunicatorMethod method)
        {
            switch (method)
            {
                case CommunicatorMethod.Remote:
                    this.communicator = NetworkCommunicator;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public void SetInputMethod(InputMethod method)
        {
            switch (method)
            {
                case InputMethod.Gamepad:
                    inputDevice = XInput;
                    break;
                case InputMethod.SteamVR:
                    inputDevice = SteamVr;
                    break;
                case InputMethod.LeapMotion:
                    inputDevice = LeapMotion;
                    break;
            }
        }

        private void Loop()
        {
            while (true)
            {
                UpdateComponents();
                System.Threading.Thread.Sleep(30);
            }
        }

        private void UpdateComponents()
        {
            InputData data = inputDevice.GetInputData();
            if (data != null)
            {
                RotationData rotData = arm.Update(data);
                if (rotData == null)
                    inputDevice.ReportBadInput();
                else
                    communicator.Update(rotData);
            }
        }
    }
}
