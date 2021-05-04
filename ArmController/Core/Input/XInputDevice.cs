using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.XInput;

namespace ArmController.Core.Input
{
    public class XInputDevice : IInputDevice
    {
        public const double DefaultPositonSpeed = 30; // m/s
        public const double DefaultRotationSpeed = 30; // degrees/s
        public const int DefaultStickDeadzone = 30; // out of 32767
        public const int DefaultTriggerDeadzone = 5; // out of 255

        private readonly Stopwatch timer = new Stopwatch();

        private Action<bool, string> statusListener;

        private SharpDX.XInput.Controller gamepad;

        private GamepadMovement lastMovement = new GamepadMovement();
        private GamepadMovement movement = new GamepadMovement();


        private int triggerDeadzone = DefaultTriggerDeadzone;

        private int lastPacketNumber = 0;

        public XInputDevice()
        {
            gamepad = new SharpDX.XInput.Controller(UserIndex.Any);
        }

        public int StickDeadzone { get; set; } = DefaultStickDeadzone;

        public double PositionSpeed { get; set; } = DefaultPositonSpeed;

        public double RotationSpeed { get; set; } = DefaultRotationSpeed;

        public bool InvertedPitch { get; set; } = false;

        private double Tool0Speed { get; set; } = 1;

        private double Tool1Speed { get; set; } = 1;

        public void SetSpeeds(double positionSpeed, double rotationSpeed, double tool0Speed, double tool1Speed)
        {
            this.PositionSpeed = positionSpeed;
            this.RotationSpeed = rotationSpeed;
            this.Tool0Speed = tool0Speed;
            this.Tool1Speed = tool1Speed;
        }

        public InputData GetInputData()
        {
            if (gamepad == null || !gamepad.IsConnected)
            {
                statusListener?.Invoke(false, "No gamepad detected.");
                return null;
            }

            statusListener?.Invoke(true, "Gamepad detected.");

            // Get time to adjust inputs
            if (!timer.IsRunning)
                timer.Start();
            long delta = Math.Min(timer.ElapsedMilliseconds, 500);
            timer.Restart();

            double seconds = delta / 1000.0;

            State state = gamepad.GetState();

            Gamepad gamepadState = state.Gamepad;
            int packetNumber = state.PacketNumber;

            if (packetNumber <= lastPacketNumber)
                return lastMovement.ToInputData();

            lastPacketNumber = packetNumber;

            bool reset = (gamepadState.Buttons & GamepadButtonFlags.Start) == GamepadButtonFlags.Start;
            if (reset)
            {
                return new GamepadMovement
                {
                    Reset = true
                }.ToInputData();
            }

            double leftX = (Math.Abs(gamepadState.LeftThumbX) < StickDeadzone) ? 0 : gamepadState.LeftThumbX / 32767.0;
            double leftY = (Math.Abs(gamepadState.LeftThumbY) < StickDeadzone) ? 0 : gamepadState.LeftThumbY / 32767.0;
            double leftMag = Math.Sqrt((leftX * leftX) + (leftY * leftY));

            double rightX = (Math.Abs(gamepadState.RightThumbX) < StickDeadzone) ? 0 : gamepadState.RightThumbX / 32767.0;
            double rightY = (Math.Abs(gamepadState.RightThumbY) < StickDeadzone) ? 0 : gamepadState.RightThumbY / 32767.0;
            double rightMag = Math.Sqrt((rightX * rightX) + (rightY * rightY));

            double xPosChange = leftX / leftMag;
            double yPosChange = InputFromButtons(gamepadState.Buttons, GamepadButtonFlags.DPadDown, GamepadButtonFlags.DPadUp);
            double zPosChange = leftY / leftMag;
            double pitchChange = rightY / rightMag;
            double rollChange = rightX / rightMag;
            double yawChange = InputFromButtons(gamepadState.Buttons, GamepadButtonFlags.LeftShoulder, GamepadButtonFlags.RightShoulder);

            if (InvertedPitch)
                pitchChange = -pitchChange;

            double tool0Set = (gamepadState.RightTrigger < triggerDeadzone) ? 0 : gamepadState.RightTrigger / 255.0;
            double tool1Set = (gamepadState.LeftTrigger < triggerDeadzone) ? 0 : gamepadState.LeftTrigger / 255.0;

            lastMovement = movement;
            movement = new GamepadMovement()
            {
                XPos = lastMovement.XPos + (xPosChange * PositionSpeed / 100.0 * seconds),
                YPos = lastMovement.YPos + (yPosChange * PositionSpeed / 100.0 * seconds),
                ZPos = lastMovement.ZPos + (zPosChange * PositionSpeed / 100.0 * seconds),

                Pitch = lastMovement.Pitch + (pitchChange * RotationSpeed * seconds),
                Yaw = lastMovement.Yaw + (rollChange * RotationSpeed * seconds),
                Roll = lastMovement.Roll + (yawChange * RotationSpeed * seconds),

                Tool0 = tool0Set,
                Tool1 = tool1Set,
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
            Vibration vibration = new Vibration
            {
                LeftMotorSpeed = 500
            };
            _ = gamepad.SetVibration(vibration);
        }

        public void RegisterStatusListener(Action<bool, string> action)
        {
            statusListener = action;

            if (gamepad.IsConnected)
                statusListener?.Invoke(true, "Gamepad detected.");
            else
                statusListener?.Invoke(false, "No gamepad detected.");
        }

        // Returns 0 if no flags or both flags
        // 1 if only pos flag
        // -1 if only neg flag
        private int InputFromButtons(GamepadButtonFlags buttons, GamepadButtonFlags negFlag, GamepadButtonFlags posFlag)
        {
            bool neg = (buttons & negFlag) == negFlag;
            bool pos = (buttons & posFlag) == posFlag;
            return Convert.ToInt32(pos) - Convert.ToInt32(neg);
        }
    }
}
