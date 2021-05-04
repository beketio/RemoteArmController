using System;
using System.Drawing;
using System.Windows.Forms;
using ArmController.Core;

namespace ArmController.Gui
{
    public partial class MainForm : Form
    {
        private readonly Controller controller;

        public MainForm(Controller controller) : base()
        {
            InitializeComponent();
            if (controller == null)
            {
                this.Close();
                return;
            }

            this.controller = controller;

            controller.NetworkCommunicator.RegisterStatusListener((c, s) => NetworkCommunicatorStatusUpdate(c, s));

            controller.XInput.RegisterStatusListener((c, s) => XInputStatusUpdate(c, s));
            controller.SteamVr.RegisterStatusListener((c, s) => SteamVrStatusUpdate(c, s));
            controller.LeapMotion.RegisterStatusListener((c, s) => LeapMotionStatusUpdate(c, s));
        }

        private void RemoteConnectButton_Click(object sender, EventArgs e)
        {
            string server = RemoteServerInput.Text;
            int port = decimal.ToInt32(RemotePortInput.Value);
            try
            {
                controller.RemoteConnect(server, port);
            }
            catch (ComponentException exception)
            {
                MessageBox.Show(exception.Message, "Couldn't Connect", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void RemoteDisconnectButton_Click(object sender, EventArgs e)
        {
            controller.RemoteDisconnect();
        }

        private void SteamVrStartButton_Click(object sender, EventArgs e)
        {
            try
            {
                controller.SteamVr.Connect();
            }
            catch (ComponentException exception)
            {
                MessageBox.Show(exception.Message, "SteamVR Not Started", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SteamVrStopButton_Click(object sender, EventArgs e)
        {
            controller.SteamVr.Disconnect();
        }

        private void SteamVrSettingsButton_Click(object sender, EventArgs e)
        {

        }

        private void GamepadSettings_Click(object sender, EventArgs e)
        {
            XInputSettings gamepadSettings = new XInputSettings(controller.XInput);
            gamepadSettings.ShowDialog();
            gamepadSettings.Dispose();
        }

        private void GamepadSelect_CheckedChanged(object sender, EventArgs e)
        {
            controller.SetInputMethod(InputMethod.Gamepad);
        }

        private void SteamVrSelect_CheckedChanged(object sender, EventArgs e)
        {
            controller.SetInputMethod(InputMethod.SteamVR);
        }

        private void LeapMotionSelect_CheckedChanged(object sender, EventArgs e)
        {
            controller.SetInputMethod(InputMethod.LeapMotion);
        }

        private void LeapMotionStartButton_Click(object sender, EventArgs e)
        {
            try
            {
                controller.LeapMotion.Connect();
            }
            catch (ComponentException exception)
            {
                MessageBox.Show(exception.Message, "LeapMotion Not Started", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LeapMotionStopButton_Click(object sender, EventArgs e)
        {
            controller.LeapMotion.Disconnect();
        }

        private void LeapMotionSettingsButton_Click(object sender, EventArgs e)
        {

        }

        private void NetworkCommunicatorStatusUpdate(bool connected, string status)
        {
            RemoteConnectButton.Enabled = !connected;
            RemoteDisconnectButton.Enabled = connected;
            RemoteServerInput.Enabled = !connected;
            RemotePortInput.Enabled = !connected;
            RemoteStatusLabel.Text = status;
        }

        private void SteamVrStatusUpdate(bool connected, string status)
        {
            SteamVrStartButton.Enabled = !connected;
            SteamVrStopButton.Enabled = connected;
            SteamVrStatusLabel.Text = status;
        }

        private void LeapMotionStatusUpdate(bool connected, string status)
        {
            LeapMotionStartButton.Enabled = !connected;
            LeapMotionStopButton.Enabled = connected;
            LeapMotionStatusLabel.Text = status;
        }

        private void XInputStatusUpdate(bool connected, string status)
        {
            // TODO: display status
        }
    }
}
