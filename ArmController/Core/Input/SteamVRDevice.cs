using System;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Spatial.Euclidean;
using Valve.VR;

namespace ArmController.Core.Input
{

    public class SteamVRDevice : IInputDevice
    {

        public const double DefaultMovementScale = 1;

        private Action<bool, string> statusListener;

        private CVRSystem vrSystem;
        private EVRInitError initError;
        private bool connected = false;
        private int controllerIndex = -1;

        private VRControllerState_t state;
        private TrackedDevicePose_t pose;

        private double xRotation = 30;

        private Vector3D originPosition;
        private Matrix<double> originRotation;

        public SteamVRDevice()
        {
            initError = EVRInitError.None;
        }

        public double MovementScale { get; set; } = DefaultMovementScale;

        public bool Connect()
        {
            vrSystem = OpenVR.Init(ref initError, EVRApplicationType.VRApplication_Other);
            if (initError != EVRInitError.None)
                throw new ComponentException(initError.ToString());

            // Get the controller index
            controllerIndex = (int)vrSystem.GetTrackedDeviceIndexForControllerRole(ETrackedControllerRole.RightHand);
            Console.WriteLine("Controller Index: " + controllerIndex);
            if (controllerIndex < 0)
                throw new ComponentException("Controller not found");

            statusListener?.Invoke(true, "SteamVR connected.");
            connected = true;
            return true;
        }

        public double ControllerOffset
        {
            get { return xRotation; }
            set { xRotation = value; }
        }

        public void ResetOrigin()
        {
            // TODO: do this
        }

        public InputData GetInputData()
        {
            InputData data = new InputData();

            if (connected)
            {
                // Get the controller index, if it wasn't on when we connected
                if (controllerIndex < 0)
                    controllerIndex = (int)vrSystem.GetTrackedDeviceIndexForControllerRole(ETrackedControllerRole.RightHand);

                // Update if valid controller index
                if (controllerIndex >= 0)
                    vrSystem.GetControllerStateWithPose(ETrackingUniverseOrigin.TrackingUniverseSeated, (uint)controllerIndex, ref state, 0, ref pose);
                else
                    return data;

                VREvent_t vrEvent = default;

                /*while (vrSystem.PollNextEvent(ref vrEvent, 0))
                {
                    if (vrEvent.trackedDeviceIndex == controllerIndex && vrEvent.eventType == (uint)EVREventType.VREvent_ButtonPress)
                        Console.WriteLine("Button: " + vrEvent.data.controller.button);
                }*/

                // Get Position
                data.Position = LinearUtils.ToPosition(pose.mDeviceToAbsoluteTracking);

                // Get Rotation
                Matrix<double> xRot = LinearUtils.GetXRotationMatrix(-Trig.DegreeToRadian(xRotation));
                data.Rotation = xRot * LinearUtils.ToRotation(pose.mDeviceToAbsoluteTracking);

                // Get Tools
                data.Tool0 = state.rAxis3.x;

                Console.WriteLine("pos: " + data.Position);
            }

            return data;
        }

        public void Disconnect()
        {
            connected = false;
            OpenVR.Shutdown();
            statusListener?.Invoke(false, "SteamVR not started.");
        }

        public void ReportBadInput()
        {
            // TODO: Vibrate controller
        }

        public void RegisterStatusListener(Action<bool, string> action)
        {
            statusListener = action;
        }

    }
}
