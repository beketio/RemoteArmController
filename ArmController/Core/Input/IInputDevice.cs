using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmController.Core.Input
{
    public interface IInputDevice
    {
        InputData GetInputData();

        void ReportBadInput();

        void RegisterStatusListener(Action<bool, string> action);
    }
}
