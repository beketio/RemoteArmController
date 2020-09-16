using System;
using System.Windows.Forms;
using ArmController.Core.Input;

namespace ArmController.Gui
{
    public partial class XInputSettings : Form
    {
        private readonly XInputDevice xInput;

        public XInputSettings(XInputDevice xInput)
        {
            InitializeComponent();
            if (xInput == null)
            {
                // Show error message
                this.Close();
                return;
            }

            this.xInput = xInput;
            StickDeadzoneInput.Value = xInput.StickDeadzone;
            PositionSpeedInput.Value = (decimal)xInput.PositionSpeed;
            RotationSpeedInput.Value = (decimal)xInput.RotationSpeed;
            InvertPitchCheckbox.Checked = xInput.InvertedPitch;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            xInput.StickDeadzone = (int)StickDeadzoneInput.Value;
            xInput.PositionSpeed = (double)PositionSpeedInput.Value;
            xInput.RotationSpeed = (double)RotationSpeedInput.Value;
            xInput.InvertedPitch = InvertPitchCheckbox.Checked;
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
