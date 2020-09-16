using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmController.Core.Input
{
    public interface IInputDeviceStatusListener
    {
        delegate void OnStatusChanged(bool started, string status);
    }
}
