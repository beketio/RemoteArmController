namespace ArmController.Gui
{
    partial class XInputSettings
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
            this.OKButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.StickDeadzoneInput = new System.Windows.Forms.NumericUpDown();
            this.InvertPitchCheckbox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.PositionSpeedInput = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.RotationSpeedInput = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.StickDeadzoneInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionSpeedInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotationSpeedInput)).BeginInit();
            this.SuspendLayout();
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(71, 154);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 26);
            this.OKButton.TabIndex = 5;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Location = new System.Drawing.Point(152, 154);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 26);
            this.CancelButton.TabIndex = 6;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // StickDeadzoneInput
            // 
            this.StickDeadzoneInput.Location = new System.Drawing.Point(152, 12);
            this.StickDeadzoneInput.Name = "StickDeadzoneInput";
            this.StickDeadzoneInput.Size = new System.Drawing.Size(120, 26);
            this.StickDeadzoneInput.TabIndex = 1;
            // 
            // InvertPitchCheckbox
            // 
            this.InvertPitchCheckbox.AutoSize = true;
            this.InvertPitchCheckbox.Location = new System.Drawing.Point(152, 114);
            this.InvertPitchCheckbox.Name = "InvertPitchCheckbox";
            this.InvertPitchCheckbox.Size = new System.Drawing.Size(15, 14);
            this.InvertPitchCheckbox.TabIndex = 4;
            this.InvertPitchCheckbox.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 20);
            this.label1.TabIndex = 7;
            this.label1.Text = "Stick Deadzone";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 20);
            this.label2.TabIndex = 9;
            this.label2.Text = "Position Speed";
            // 
            // PositionSpeedInput
            // 
            this.PositionSpeedInput.DecimalPlaces = 1;
            this.PositionSpeedInput.Location = new System.Drawing.Point(152, 44);
            this.PositionSpeedInput.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.PositionSpeedInput.Name = "PositionSpeedInput";
            this.PositionSpeedInput.Size = new System.Drawing.Size(120, 26);
            this.PositionSpeedInput.TabIndex = 2;
            this.PositionSpeedInput.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(121, 20);
            this.label3.TabIndex = 11;
            this.label3.Text = "Rotation Speed";
            // 
            // RotationSpeedInput
            // 
            this.RotationSpeedInput.DecimalPlaces = 1;
            this.RotationSpeedInput.Location = new System.Drawing.Point(152, 76);
            this.RotationSpeedInput.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.RotationSpeedInput.Name = "RotationSpeedInput";
            this.RotationSpeedInput.Size = new System.Drawing.Size(120, 26);
            this.RotationSpeedInput.TabIndex = 3;
            this.RotationSpeedInput.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(58, 110);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 20);
            this.label4.TabIndex = 12;
            this.label4.Text = "Invert Pitch";
            // 
            // XInputSettings
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(299, 198);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.RotationSpeedInput);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.PositionSpeedInput);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.InvertPitchCheckbox);
            this.Controls.Add(this.StickDeadzoneInput);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OKButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "XInputSettings";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Gamepad Settings";
            ((System.ComponentModel.ISupportInitialize)(this.StickDeadzoneInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionSpeedInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotationSpeedInput)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.NumericUpDown StickDeadzoneInput;
        private System.Windows.Forms.CheckBox InvertPitchCheckbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown PositionSpeedInput;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown RotationSpeedInput;
        private System.Windows.Forms.Label label4;
    }
}