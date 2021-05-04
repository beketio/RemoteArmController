using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using SharpDX.DirectInput;

namespace ArmController.Core.Input
{
    internal class GamepadDevice : IInputDevice, IDisposable
    {
        private readonly Stopwatch timer = new Stopwatch();

        private readonly DirectInput directInput;
        private readonly List<DeviceInstance> devices = new List<DeviceInstance>();

        private Action<bool, string> statusListener;

        private Joystick gamepad = null;

        private GamepadMovement lastMovement = new GamepadMovement();
        private GamepadMovement movement = new GamepadMovement();
        private double positionSpeed = 1;
        private double rotationSpeed = 1;
        private double tool0Speed = 1;
        private double tool1Speed = 1;

        public GamepadDevice()
        {
            directInput = new DirectInput();
            RefreshDevices();
            SetDevice(0);
        }

        public void SetSpeeds(double positionSpeed, double rotationSpeed, double tool0Speed, double tool1Speed)
        {
            this.positionSpeed = positionSpeed;
            this.rotationSpeed = rotationSpeed;
            this.tool0Speed = tool0Speed;
            this.tool1Speed = tool1Speed;
        }

        public void RefreshDevices()
        {
            devices.Clear();

            foreach (DeviceInstance deviceInstance in directInput.GetDevices(DeviceType.Gamepad,
                        DeviceEnumerationFlags.AllDevices))
                devices.Add(deviceInstance);

            foreach (DeviceInstance deviceInstance in directInput.GetDevices(DeviceType.Joystick,
                        DeviceEnumerationFlags.AllDevices))
                devices.Add(deviceInstance);

            foreach (DeviceInstance deviceInstance in directInput.GetDevices(DeviceType.Device,
                        DeviceEnumerationFlags.AllDevices))
                devices.Add(deviceInstance);

            Console.WriteLine("Found " + devices.Count + " devices.");
            foreach (DeviceInstance dev in devices)
                Console.WriteLine(dev.InstanceName + ", " + dev.ProductName);
        }

        public string[] GetDevices()
        {
            string[] deviceNames = new string[devices.Count];
            for (int i = 0; i < devices.Count; i++)
                deviceNames[i] = devices[i].InstanceName;
            return deviceNames;
        }

        public void SetDevice(int index)
        {
            gamepad?.Unacquire();
            gamepad?.Dispose();
            gamepad = null;
            if (index < devices.Count)
            {
                Console.WriteLine("Found Joystick/Gamepad with GUID: {0}", devices[index].InstanceGuid);
                gamepad = new Joystick(directInput, devices[index].InstanceGuid);
                gamepad.Properties.BufferSize = 128;
                gamepad.Acquire();

                // Query all suported ForceFeedback effects
                var allEffects = gamepad.GetEffects();
                foreach (var effectInfo in allEffects)
                    Console.WriteLine("Effect available {0}", effectInfo.Name);
            }
        }

        public InputData GetInputData()
        {
            if (gamepad == null)
                return null;

            // Get time to adjust inputs
            if (!timer.IsRunning)
                timer.Start();
            long delta = Math.Min(timer.ElapsedMilliseconds, 500);
            timer.Restart();

            double seconds = delta / 1000.0;

            // Retrieve data from the gamepad
            gamepad.Poll();
            JoystickUpdate[] updates = gamepad.GetBufferedData();
            foreach (JoystickUpdate update in updates)
                Console.WriteLine(update);

            double xPosChange = 0;
            double yPosChange = 0;
            double zPosChange = 0;
            double pitchChange = 0;
            double rollChange = 0;
            double yawChange = 0;
            double tool0Change = 0;
            double tool1Change = 0;

            lastMovement = movement;
            movement = new GamepadMovement()
            {
                XPos = lastMovement.XPos + (xPosChange * positionSpeed * seconds),
                YPos = lastMovement.YPos + (yPosChange * positionSpeed * seconds),
                ZPos = lastMovement.ZPos + (zPosChange * positionSpeed * seconds),

                Pitch = lastMovement.Pitch + (pitchChange * rotationSpeed * seconds),
                Yaw = lastMovement.Yaw + (rollChange * rotationSpeed * seconds),
                Roll = lastMovement.Roll + (yawChange * rotationSpeed * seconds),

                Tool0 = lastMovement.Tool0 + (tool0Change * tool0Speed * seconds),
                Tool1 = lastMovement.Tool1 + (tool1Change * tool1Speed * seconds),
            };

            // Rotation loops back around
            if (movement.Pitch < -180)
                movement.Pitch += 360;
            else if (movement.Pitch > 180)
                movement.Pitch -= 360;

            if (movement.Yaw < -180)
                movement.Yaw += 360;
            else if (movement.Yaw > 180)
                movement.Yaw -= 360;

            if (movement.Roll < -180)
                movement.Roll += 360;
            else if (movement.Roll > 180)
                movement.Roll -= 360;

            // Tools stay within 0 and 1 inclusive
            if (movement.Tool0 > 1)
                movement.Tool0 = 1;
            else if (movement.Tool0 < 0)
                movement.Tool0 = 0;

            if (movement.Tool1 > 1)
                movement.Tool1 = 1;
            else if (movement.Tool1 < 0)
                movement.Tool1 = 0;

            return movement.ToInputData();
        }

        public void ReportBadInput()
        {
            movement = lastMovement;

            // Vibrate?
        }

        public void Dispose()
        {
            gamepad?.Dispose();
            directInput?.Dispose();
        }

        public void RegisterStatusListener(Action<bool, string> listener)
        {
            statusListener = listener;
        }
    }
}