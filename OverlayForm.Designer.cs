namespace ParaParaView
{
    partial class OverlayForm
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
            this.components = new System.ComponentModel.Container();
            this.FadeInTimer = new System.Windows.Forms.Timer(this.components);
            this.FadeOutTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // FadeInTimer
            // 
            this.FadeInTimer.Interval = 10;
            this.FadeInTimer.Tick += new System.EventHandler(this.FadeInTimer_Tick);
            // 
            // FadeOutTimer
            // 
            this.FadeOutTimer.Interval = 10;
            this.FadeOutTimer.Tick += new System.EventHandler(this.FadeOutTimer_Tick);
            // 
            // OverlayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Lime;
            this.ClientSize = new System.Drawing.Size(640, 480);
            this.Enabled = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "OverlayForm";
            this.ShowInTaskbar = false;
            this.Text = "OverlayForm";
            this.TransparencyKey = System.Drawing.Color.Lime;
            this.Shown += new System.EventHandler(this.OverlayForm_Shown);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer FadeInTimer;
        private System.Windows.Forms.Timer FadeOutTimer;
    }
}