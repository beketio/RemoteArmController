namespace ArmController.Gui
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.RemoteConnectButton = new System.Windows.Forms.Button();
            this.RemoteServerInput = new System.Windows.Forms.TextBox();
            this.RemotePortLabel = new System.Windows.Forms.Label();
            this.RemoteServerLabel = new System.Windows.Forms.Label();
            this.RemoteGroup = new System.Windows.Forms.GroupBox();
            this.RemotePortInput = new System.Windows.Forms.NumericUpDown();
            this.RemoteDisconnectButton = new System.Windows.Forms.Button();
            this.RemoteStatusLabel = new System.Windows.Forms.Label();
            this.ControlGroup = new System.Windows.Forms.GroupBox();
            this.LeapMotionGroup = new System.Windows.Forms.GroupBox();
            this.LeapMotionStatusLabel = new System.Windows.Forms.Label();
            this.LeapMotionSettingsButton = new System.Windows.Forms.Button();
            this.LeapMotionStopButton = new System.Windows.Forms.Button();
            this.LeapMotionStartButton = new System.Windows.Forms.Button();
            this.InputGroup = new System.Windows.Forms.GroupBox();
            this.LeapMotionSelect = new System.Windows.Forms.RadioButton();
            this.SteamVrSelect = new System.Windows.Forms.RadioButton();
            this.GamepadSelect = new System.Windows.Forms.RadioButton();
            this.GamepadGroup = new System.Windows.Forms.GroupBox();
            this.GamepadSettings = new System.Windows.Forms.Button();
            this.SteamVrGroup = new System.Windows.Forms.GroupBox();
            this.SteamVrSettingsButton = new System.Windows.Forms.Button();
            this.SteamVrStatusLabel = new System.Windows.Forms.Label();
            this.SteamVrStopButton = new System.Windows.Forms.Button();
            this.SteamVrStartButton = new System.Windows.Forms.Button();
            this.ViewGroup = new System.Windows.Forms.GroupBox();
            this.RemoteGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RemotePortInput)).BeginInit();
            this.ControlGroup.SuspendLayout();
            this.LeapMotionGroup.SuspendLayout();
            this.InputGroup.SuspendLayout();
            this.GamepadGroup.SuspendLayout();
            this.SteamVrGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // RemoteConnectButton
            // 
            this.RemoteConnectButton.Location = new System.Drawing.Point(213, 17);
            this.RemoteConnectButton.Name = "RemoteConnectButton";
            this.RemoteConnectButton.Size = new System.Drawing.Size(75, 23);
            this.RemoteConnectButton.TabIndex = 2;
            this.RemoteConnectButton.Text = "Connect";
            this.RemoteConnectButton.UseVisualStyleBackColor = true;
            this.RemoteConnectButton.Click += new System.EventHandler(this.RemoteConnectButton_Click);
            // 
            // RemoteServerInput
            // 
            this.RemoteServerInput.Location = new System.Drawing.Point(70, 19);
            this.RemoteServerInput.Name = "RemoteServerInput";
            this.RemoteServerInput.Size = new System.Drawing.Size(137, 20);
            this.RemoteServerInput.TabIndex = 0;
            // 
            // RemotePortLabel
            // 
            this.RemotePortLabel.AutoSize = true;
            this.RemotePortLabel.Location = new System.Drawing.Point(10, 51);
            this.RemotePortLabel.Name = "RemotePortLabel";
            this.RemotePortLabel.Size = new System.Drawing.Size(26, 13);
            this.RemotePortLabel.TabIndex = 4;
            this.RemotePortLabel.Text = "Port";
            // 
            // RemoteServerLabel
            // 
            this.RemoteServerLabel.AutoSize = true;
            this.RemoteServerLabel.Location = new System.Drawing.Point(10, 22);
            this.RemoteServerLabel.Name = "RemoteServerLabel";
            this.RemoteServerLabel.Size = new System.Drawing.Size(38, 13);
            this.RemoteServerLabel.TabIndex = 5;
            this.RemoteServerLabel.Text = "Server";
            // 
            // RemoteGroup
            // 
            this.RemoteGroup.Controls.Add(this.RemotePortInput);
            this.RemoteGroup.Controls.Add(this.RemoteDisconnectButton);
            this.RemoteGroup.Controls.Add(this.RemoteStatusLabel);
            this.RemoteGroup.Controls.Add(this.RemoteServerInput);
            this.RemoteGroup.Controls.Add(this.RemoteServerLabel);
            this.RemoteGroup.Controls.Add(this.RemoteConnectButton);
            this.RemoteGroup.Controls.Add(this.RemotePortLabel);
            this.RemoteGroup.Location = new System.Drawing.Point(12, 12);
            this.RemoteGroup.Name = "RemoteGroup";
            this.RemoteGroup.Size = new System.Drawing.Size(294, 105);
            this.RemoteGroup.TabIndex = 8;
            this.RemoteGroup.TabStop = false;
            this.RemoteGroup.Text = "Remote Connection";
            // 
            // RemotePortInput
            // 
            this.RemotePortInput.Location = new System.Drawing.Point(70, 49);
            this.RemotePortInput.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.RemotePortInput.Name = "RemotePortInput";
            this.RemotePortInput.Size = new System.Drawing.Size(137, 20);
            this.RemotePortInput.TabIndex = 1;
            this.RemotePortInput.Value = new decimal(new int[] {
            4480,
            0,
            0,
            0});
            // 
            // RemoteDisconnectButton
            // 
            this.RemoteDisconnectButton.Enabled = false;
            this.RemoteDisconnectButton.Location = new System.Drawing.Point(213, 46);
            this.RemoteDisconnectButton.Name = "RemoteDisconnectButton";
            this.RemoteDisconnectButton.Size = new System.Drawing.Size(75, 23);
            this.RemoteDisconnectButton.TabIndex = 3;
            this.RemoteDisconnectButton.Text = "Disconnect";
            this.RemoteDisconnectButton.UseVisualStyleBackColor = true;
            this.RemoteDisconnectButton.Click += new System.EventHandler(this.RemoteDisconnectButton_Click);
            // 
            // RemoteStatusLabel
            // 
            this.RemoteStatusLabel.AutoSize = true;
            this.RemoteStatusLabel.Location = new System.Drawing.Point(10, 78);
            this.RemoteStatusLabel.Name = "RemoteStatusLabel";
            this.RemoteStatusLabel.Size = new System.Drawing.Size(79, 13);
            this.RemoteStatusLabel.TabIndex = 6;
            this.RemoteStatusLabel.Text = "Not Connected";
            // 
            // ControlGroup
            // 
            this.ControlGroup.Controls.Add(this.LeapMotionGroup);
            this.ControlGroup.Controls.Add(this.InputGroup);
            this.ControlGroup.Controls.Add(this.GamepadGroup);
            this.ControlGroup.Controls.Add(this.SteamVrGroup);
            this.ControlGroup.Location = new System.Drawing.Point(12, 123);
            this.ControlGroup.Name = "ControlGroup";
            this.ControlGroup.Size = new System.Drawing.Size(294, 235);
            this.ControlGroup.TabIndex = 9;
            this.ControlGroup.TabStop = false;
            this.ControlGroup.Text = "Control";
            // 
            // LeapMotionGroup
            // 
            this.LeapMotionGroup.Controls.Add(this.LeapMotionStatusLabel);
            this.LeapMotionGroup.Controls.Add(this.LeapMotionSettingsButton);
            this.LeapMotionGroup.Controls.Add(this.LeapMotionStopButton);
            this.LeapMotionGroup.Controls.Add(this.LeapMotionStartButton);
            this.LeapMotionGroup.Cursor = System.Windows.Forms.Cursors.Default;
            this.LeapMotionGroup.Location = new System.Drawing.Point(5, 164);
            this.LeapMotionGroup.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.LeapMotionGroup.Name = "LeapMotionGroup";
            this.LeapMotionGroup.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.LeapMotionGroup.Size = new System.Drawing.Size(281, 66);
            this.LeapMotionGroup.TabIndex = 4;
            this.LeapMotionGroup.TabStop = false;
            this.LeapMotionGroup.Text = "Leap Motion";
            // 
            // LeapMotionStatusLabel
            // 
            this.LeapMotionStatusLabel.AutoSize = true;
            this.LeapMotionStatusLabel.Location = new System.Drawing.Point(6, 47);
            this.LeapMotionStatusLabel.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.LeapMotionStatusLabel.Name = "LeapMotionStatusLabel";
            this.LeapMotionStatusLabel.Size = new System.Drawing.Size(61, 13);
            this.LeapMotionStatusLabel.TabIndex = 6;
            this.LeapMotionStatusLabel.Text = "Not Started";
            // 
            // LeapMotionSettingsButton
            // 
            this.LeapMotionSettingsButton.Location = new System.Drawing.Point(201, 18);
            this.LeapMotionSettingsButton.Name = "LeapMotionSettingsButton";
            this.LeapMotionSettingsButton.Size = new System.Drawing.Size(75, 23);
            this.LeapMotionSettingsButton.TabIndex = 5;
            this.LeapMotionSettingsButton.Text = "Settings";
            this.LeapMotionSettingsButton.UseVisualStyleBackColor = true;
            this.LeapMotionSettingsButton.Click += new System.EventHandler(this.LeapMotionSettingsButton_Click);
            // 
            // LeapMotionStopButton
            // 
            this.LeapMotionStopButton.Enabled = false;
            this.LeapMotionStopButton.Location = new System.Drawing.Point(104, 18);
            this.LeapMotionStopButton.Name = "LeapMotionStopButton";
            this.LeapMotionStopButton.Size = new System.Drawing.Size(75, 23);
            this.LeapMotionStopButton.TabIndex = 4;
            this.LeapMotionStopButton.Text = "Stop";
            this.LeapMotionStopButton.UseVisualStyleBackColor = true;
            this.LeapMotionStopButton.Click += new System.EventHandler(this.LeapMotionStopButton_Click);
            // 
            // LeapMotionStartButton
            // 
            this.LeapMotionStartButton.Location = new System.Drawing.Point(6, 18);
            this.LeapMotionStartButton.Name = "LeapMotionStartButton";
            this.LeapMotionStartButton.Size = new System.Drawing.Size(75, 23);
            this.LeapMotionStartButton.TabIndex = 3;
            this.LeapMotionStartButton.Text = "Start";
            this.LeapMotionStartButton.UseVisualStyleBackColor = true;
            this.LeapMotionStartButton.Click += new System.EventHandler(this.LeapMotionStartButton_Click);
            // 
            // InputGroup
            // 
            this.InputGroup.Controls.Add(this.LeapMotionSelect);
            this.InputGroup.Controls.Add(this.SteamVrSelect);
            this.InputGroup.Controls.Add(this.GamepadSelect);
            this.InputGroup.Location = new System.Drawing.Point(5, 19);
            this.InputGroup.Name = "InputGroup";
            this.InputGroup.Size = new System.Drawing.Size(182, 63);
            this.InputGroup.TabIndex = 3;
            this.InputGroup.TabStop = false;
            this.InputGroup.Text = "Input";
            // 
            // LeapMotionSelect
            // 
            this.LeapMotionSelect.AutoSize = true;
            this.LeapMotionSelect.Location = new System.Drawing.Point(83, 40);
            this.LeapMotionSelect.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.LeapMotionSelect.Name = "LeapMotionSelect";
            this.LeapMotionSelect.Size = new System.Drawing.Size(84, 17);
            this.LeapMotionSelect.TabIndex = 5;
            this.LeapMotionSelect.TabStop = true;
            this.LeapMotionSelect.Text = "Leap Motion";
            this.LeapMotionSelect.UseVisualStyleBackColor = true;
            this.LeapMotionSelect.CheckedChanged += new System.EventHandler(this.LeapMotionSelect_CheckedChanged);
            // 
            // SteamVrSelect
            // 
            this.SteamVrSelect.AutoSize = true;
            this.SteamVrSelect.Location = new System.Drawing.Point(83, 19);
            this.SteamVrSelect.Name = "SteamVrSelect";
            this.SteamVrSelect.Size = new System.Drawing.Size(70, 17);
            this.SteamVrSelect.TabIndex = 4;
            this.SteamVrSelect.Text = "SteamVR";
            this.SteamVrSelect.UseVisualStyleBackColor = true;
            this.SteamVrSelect.CheckedChanged += new System.EventHandler(this.SteamVrSelect_CheckedChanged);
            // 
            // GamepadSelect
            // 
            this.GamepadSelect.AutoSize = true;
            this.GamepadSelect.Checked = true;
            this.GamepadSelect.Location = new System.Drawing.Point(6, 19);
            this.GamepadSelect.Name = "GamepadSelect";
            this.GamepadSelect.Size = new System.Drawing.Size(71, 17);
            this.GamepadSelect.TabIndex = 3;
            this.GamepadSelect.TabStop = true;
            this.GamepadSelect.Text = "Gamepad";
            this.GamepadSelect.UseVisualStyleBackColor = true;
            this.GamepadSelect.CheckedChanged += new System.EventHandler(this.GamepadSelect_CheckedChanged);
            // 
            // GamepadGroup
            // 
            this.GamepadGroup.Controls.Add(this.GamepadSettings);
            this.GamepadGroup.Location = new System.Drawing.Point(195, 19);
            this.GamepadGroup.Name = "GamepadGroup";
            this.GamepadGroup.Size = new System.Drawing.Size(93, 55);
            this.GamepadGroup.TabIndex = 2;
            this.GamepadGroup.TabStop = false;
            this.GamepadGroup.Text = "Gamepad";
            // 
            // GamepadSettings
            // 
            this.GamepadSettings.Location = new System.Drawing.Point(11, 19);
            this.GamepadSettings.Name = "GamepadSettings";
            this.GamepadSettings.Size = new System.Drawing.Size(75, 23);
            this.GamepadSettings.TabIndex = 1;
            this.GamepadSettings.Text = "Settings";
            this.GamepadSettings.UseVisualStyleBackColor = true;
            this.GamepadSettings.Click += new System.EventHandler(this.GamepadSettings_Click);
            // 
            // SteamVrGroup
            // 
            this.SteamVrGroup.Controls.Add(this.SteamVrSettingsButton);
            this.SteamVrGroup.Controls.Add(this.SteamVrStatusLabel);
            this.SteamVrGroup.Controls.Add(this.SteamVrStopButton);
            this.SteamVrGroup.Controls.Add(this.SteamVrStartButton);
            this.SteamVrGroup.Location = new System.Drawing.Point(5, 88);
            this.SteamVrGroup.Name = "SteamVrGroup";
            this.SteamVrGroup.Size = new System.Drawing.Size(281, 71);
            this.SteamVrGroup.TabIndex = 1;
            this.SteamVrGroup.TabStop = false;
            this.SteamVrGroup.Text = "SteamVR";
            // 
            // SteamVrSettingsButton
            // 
            this.SteamVrSettingsButton.Location = new System.Drawing.Point(201, 19);
            this.SteamVrSettingsButton.Name = "SteamVrSettingsButton";
            this.SteamVrSettingsButton.Size = new System.Drawing.Size(75, 23);
            this.SteamVrSettingsButton.TabIndex = 2;
            this.SteamVrSettingsButton.Text = "Settings";
            this.SteamVrSettingsButton.UseVisualStyleBackColor = true;
            this.SteamVrSettingsButton.Click += new System.EventHandler(this.SteamVrSettingsButton_Click);
            // 
            // SteamVrStatusLabel
            // 
            this.SteamVrStatusLabel.AutoSize = true;
            this.SteamVrStatusLabel.Location = new System.Drawing.Point(6, 51);
            this.SteamVrStatusLabel.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.SteamVrStatusLabel.Name = "SteamVrStatusLabel";
            this.SteamVrStatusLabel.Size = new System.Drawing.Size(61, 13);
            this.SteamVrStatusLabel.TabIndex = 2;
            this.SteamVrStatusLabel.Text = "Not Started";
            // 
            // SteamVrStopButton
            // 
            this.SteamVrStopButton.Enabled = false;
            this.SteamVrStopButton.Location = new System.Drawing.Point(104, 19);
            this.SteamVrStopButton.Name = "SteamVrStopButton";
            this.SteamVrStopButton.Size = new System.Drawing.Size(75, 23);
            this.SteamVrStopButton.TabIndex = 1;
            this.SteamVrStopButton.Text = "Stop";
            this.SteamVrStopButton.UseVisualStyleBackColor = true;
            this.SteamVrStopButton.Click += new System.EventHandler(this.SteamVrStopButton_Click);
            // 
            // SteamVrStartButton
            // 
            this.SteamVrStartButton.Location = new System.Drawing.Point(6, 19);
            this.SteamVrStartButton.Name = "SteamVrStartButton";
            this.SteamVrStartButton.Size = new System.Drawing.Size(75, 23);
            this.SteamVrStartButton.TabIndex = 0;
            this.SteamVrStartButton.Text = "Start";
            this.SteamVrStartButton.UseVisualStyleBackColor = true;
            this.SteamVrStartButton.Click += new System.EventHandler(this.SteamVrStartButton_Click);
            // 
            // ViewGroup
            // 
            this.ViewGroup.Location = new System.Drawing.Point(312, 12);
            this.ViewGroup.Name = "ViewGroup";
            this.ViewGroup.Size = new System.Drawing.Size(300, 300);
            this.ViewGroup.TabIndex = 10;
            this.ViewGroup.TabStop = false;
            this.ViewGroup.Text = "View";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(628, 369);
            this.Controls.Add(this.ViewGroup);
            this.Controls.Add(this.ControlGroup);
            this.Controls.Add(this.RemoteGroup);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(640, 360);
            this.Name = "MainForm";
            this.Text = "`";
            this.RemoteGroup.ResumeLayout(false);
            this.RemoteGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RemotePortInput)).EndInit();
            this.ControlGroup.ResumeLayout(false);
            this.LeapMotionGroup.ResumeLayout(false);
            this.LeapMotionGroup.PerformLayout();
            this.InputGroup.ResumeLayout(false);
            this.InputGroup.PerformLayout();
            this.GamepadGroup.ResumeLayout(false);
            this.SteamVrGroup.ResumeLayout(false);
            this.SteamVrGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button RemoteConnectButton;
        private System.Windows.Forms.TextBox RemoteServerInput;
        private System.Windows.Forms.Label RemotePortLabel;
        private System.Windows.Forms.Label RemoteServerLabel;
        private System.Windows.Forms.GroupBox RemoteGroup;
        private System.Windows.Forms.GroupBox ControlGroup;
        private System.Windows.Forms.GroupBox ViewGroup;
        private System.Windows.Forms.GroupBox SteamVrGroup;
        private System.Windows.Forms.Button SteamVrStopButton;
        private System.Windows.Forms.Button SteamVrStartButton;
        private System.Windows.Forms.Button RemoteDisconnectButton;
        private System.Windows.Forms.Button SteamVrSettingsButton;
        private System.Windows.Forms.Label SteamVrStatusLabel;
        private System.Windows.Forms.GroupBox GamepadGroup;
        private System.Windows.Forms.Button GamepadSettings;
        private System.Windows.Forms.NumericUpDown RemotePortInput;
        private System.Windows.Forms.Label RemoteStatusLabel;
        private System.Windows.Forms.GroupBox InputGroup;
        private System.Windows.Forms.RadioButton SteamVrSelect;
        private System.Windows.Forms.RadioButton GamepadSelect;
        private System.Windows.Forms.GroupBox LeapMotionGroup;
        private System.Windows.Forms.Label LeapMotionStatusLabel;
        private System.Windows.Forms.Button LeapMotionSettingsButton;
        private System.Windows.Forms.Button LeapMotionStopButton;
        private System.Windows.Forms.Button LeapMotionStartButton;
        private System.Windows.Forms.RadioButton LeapMotionSelect;
    }
}

