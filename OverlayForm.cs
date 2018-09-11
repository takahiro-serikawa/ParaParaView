using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParaParaView
{
    public partial class OverlayForm: Form
    {
        Form real { get; set; }

        public OverlayForm()
        {
            InitializeComponent();
        }

        public OverlayForm(Form form)
        {
            InitializeComponent();

            if (form != null) {
                real = form;
                real.Move += Form_Move;
                real.Resize += Form_Resize;
                real.Shown += Form_Move;
                real.Shown += Form_Resize;
                real.AddOwnedForm(this);
            }
            //Show();
        }

        private void Form_Move(object sender, EventArgs e)
        {
            this.Location = real.PointToScreen(new Point(0, 0));
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            this.Size = real.ClientSize;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x00000020;
                return cp;
            }
        }

        /// <summary>
        /// fade in AboutBox
        /// </summary>
        /// <param name="bounds"></param>
        public void FadeIn()
        {
            this.Location = real.PointToScreen(new Point(0, 0));
            this.Size = real.ClientSize;

            this.Opacity = 0;
            //this.Show();
            this.Visible = true;
            real.Focus();
            //this.TopMost = true;
            //this.TopMost = false;
            fade_start_tc = Environment.TickCount;
            FadeInTimer.Enabled = true;
        }

        /// <summary>
        /// fade out AboutBox, and hide.
        /// </summary>
        public void FadeOut()
        {
            this.Opacity = DEF_OPACITY;
            fade_start_tc = Environment.TickCount;
            FadeOutTimer.Enabled = true;
        }

        //const float DEF_OPACITY = 0.75f;
        const float DEF_OPACITY = 0.67f;
        const int FADE_IN_MSEC = 2000;
        const int FADE_OUT_MSEC = 500;
        int fade_start_tc;

        private void FadeInTimer_Tick(object sender, EventArgs e)
        {
            int tc = Environment.TickCount - fade_start_tc;
            if (tc > FADE_IN_MSEC) {
                FadeInTimer.Enabled = false;
                this.Opacity = DEF_OPACITY;
            } else
                this.Opacity = DEF_OPACITY*tc/FADE_IN_MSEC;
        }

        private void FadeOutTimer_Tick(object sender, EventArgs e)
        {
            int tc = Environment.TickCount - fade_start_tc;
            if (tc > FADE_OUT_MSEC) {
                FadeOutTimer.Enabled = false;
                this.Hide();
            } else
                this.Opacity = DEF_OPACITY - DEF_OPACITY*tc/FADE_OUT_MSEC;
        }

    }
}
