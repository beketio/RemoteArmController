using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using SharpDX.DirectInput;

namespace ArmController.Core.Input
{
    public class GamepadBindings
    {

        private Dictionary<GamepadInput, Binding> inputMap = new Dictionary<GamepadInput, Binding>();
        private Dictionary<JoystickOffset, Binding> bindingMap = new Dictionary<JoystickOffset, Binding>();

        public GamepadBindings()
        {

        }

        public void RegisterPositive(GamepadInput input, JoystickOffset offset)
        {
            Binding bind = inputMap[input];
            bind.PosBinding = offset;
        }

        public void RegisterNegative(GamepadInput input, JoystickOffset offset)
        {
            Binding bind = inputMap[input];
            bind.NegBinding = offset;
        }

        public void RegisterAnalog(GamepadInput input, JoystickOffset offset)
        {
            Binding bind = inputMap[input];
            bind.AnalogBinding = offset;
        }

        private class Binding
        {
            public GamepadInput Input { get; }

            public JoystickOffset PosBinding { get; set; }

            public JoystickOffset NegBinding { get; set; }

            public JoystickOffset AnalogBinding { get; set; }

            public Binding(GamepadInput input)
            {
                this.Input = input;
            }

            public double GetValue(JoystickUpdate update)
            {
                return 0;
            }
        }
    }

    
}
