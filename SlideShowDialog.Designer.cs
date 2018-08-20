namespace ParaParaView
{
    partial class SlideShowDialog
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
            if (disposing && (components != null)) {
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
            this.OkButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.IntervalLabel = new System.Windows.Forms.Label();
            this.SlideShowIntervalText = new System.Windows.Forms.TextBox();
            this.SecLabel = new System.Windows.Forms.Label();
            this.ShuffleCheck = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // OkButton
            // 
            this.OkButton.Location = new System.Drawing.Point(27, 103);
            this.OkButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(87, 29);
            this.OkButton.TabIndex = 0;
            this.OkButton.Text = "&OK";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(131, 103);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(87, 29);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // IntervalLabel
            // 
            this.IntervalLabel.AutoSize = true;
            this.IntervalLabel.Location = new System.Drawing.Point(24, 15);
            this.IntervalLabel.Name = "IntervalLabel";
            this.IntervalLabel.Size = new System.Drawing.Size(56, 15);
            this.IntervalLabel.TabIndex = 3;
            this.IntervalLabel.Text = "interval:";
            // 
            // SlideShowIntervalText
            // 
            this.SlideShowIntervalText.Location = new System.Drawing.Point(86, 12);
            this.SlideShowIntervalText.Name = "SlideShowIntervalText";
            this.SlideShowIntervalText.Size = new System.Drawing.Size(100, 23);
            this.SlideShowIntervalText.TabIndex = 3;
            this.SlideShowIntervalText.Text = "2.500";
            this.SlideShowIntervalText.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.SlideShowIntervalText.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // SecLabel
            // 
            this.SecLabel.AutoSize = true;
            this.SecLabel.Location = new System.Drawing.Point(192, 15);
            this.SecLabel.Name = "SecLabel";
            this.SecLabel.Size = new System.Drawing.Size(26, 15);
            this.SecLabel.TabIndex = 4;
            this.SecLabel.Text = "sec";
            // 
            // ShuffleCheck
            // 
            this.ShuffleCheck.AutoSize = true;
            this.ShuffleCheck.Checked = true;
            this.ShuffleCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShuffleCheck.Location = new System.Drawing.Point(27, 41);
            this.ShuffleCheck.Name = "ShuffleCheck";
            this.ShuffleCheck.Size = new System.Drawing.Size(64, 19);
            this.ShuffleCheck.TabIndex = 5;
            this.ShuffleCheck.Text = "shuffle";
            this.ShuffleCheck.UseVisualStyleBackColor = true;
            // 
            // SlideShowDialog
            // 
            this.AcceptButton = this.OkButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(250, 145);
            this.ControlBox = false;
            this.Controls.Add(this.ShuffleCheck);
            this.Controls.Add(this.SecLabel);
            this.Controls.Add(this.SlideShowIntervalText);
            this.Controls.Add(this.IntervalLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.OkButton);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "SlideShowDialog";
            this.Opacity = 0.9D;
            this.ShowInTaskbar = false;
            this.Text = "Slide Show";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label IntervalLabel;
        private System.Windows.Forms.TextBox SlideShowIntervalText;
        private System.Windows.Forms.Label SecLabel;
        private System.Windows.Forms.CheckBox ShuffleCheck;
    }
}