using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmController.Core
{
    interface IGamepadInputListener
    {
        double XPos { get; set; }

        double YPos { get; set; }

        double ZPos { get; set; }

        double Pitch { get; set; }

        double Roll { get; set; }

        double Yaw { get; set; }

        double Tool0 { get; set; }

        double Tool1 { get; set; }
    }
}
