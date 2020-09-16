using ArmController.Core.Arm;
using System;

namespace ArmController.Core.Communication
{
    public interface ICommunicator
    {
        bool Update(RotationData data);

        void RegisterStatusListener(Action<bool, string> action);
    }
}
