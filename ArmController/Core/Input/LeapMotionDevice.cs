using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmController.Core.Input
{
    public class LeapMotionDevice : IInputDevice
    {
        private Action<bool, string> statusListener;

        public LeapMotionDevice()
        {
        }

        public void Connect()
        {
            throw new ComponentException("LeapMotion support coming soon...");
            statusListener.Invoke(true, "LeapMotion started.");
        }

        public void Disconnect()
        {
            statusListener.Invoke(false, "LeapMotion not started.");
        }

        public InputData GetInputData()
        {
            return null;
        }

        public void ReportBadInput()
        {
            // TODO: Beep?
        }

        public void RegisterStatusListener(Action<bool, string> listener)
        {
            statusListener = listener;
        }
    }
}
