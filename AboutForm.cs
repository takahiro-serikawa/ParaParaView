using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;

namespace ParaParaView
{
    /// <summary>
    /// display version, lisence and usage.
    /// </summary>
    public partial class AboutForm: Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AboutForm()
        {
            InitializeComponent();

            LoadLicense();
        }

        public Dictionary<string, string> Params = new Dictionary<string, string>();

        void LoadLicense()
        {
            string name = this.GetType().Namespace+@".license.html";
            var asm = System.Reflection.Assembly.GetExecutingAssembly();
            var stream = asm.GetManifestResourceStream(name);
            if (stream != null) {
                using (var sr = new StreamReader(stream, Encoding.UTF8))
                    webBrowser1.DocumentText = sr.ReadToEnd();
                stream.Dispose();
            }
        }

        /// <summary>
        /// fade in AboutBox
        /// </summary>
        /// <param name="bounds"></param>
        public void FadeIn(Rectangle bounds)
        {
            foreach (var p in Params) {
                var elem = webBrowser1.Document.GetElementById(p.Key);
                if (elem != null)
                    elem.InnerHtml = p.Value;
            }

            this.Bounds = bounds;

            this.Opacity = 0;
            fade_start_tc = Environment.TickCount;
            this.Show();
            this.Focus();
            timer1.Enabled = true;
        }

        /// <summary>
        /// fade out AboutBox, and hide.
        /// </summary>
        public void FadeOut()
        {
            this.Opacity = DEF_OPACITY;
            fade_start_tc = Environment.TickCount;
            timer2.Enabled = true;
        }

        //const float DEF_OPACITY = 0.75f;
        const float DEF_OPACITY = 0.67f;
        const int FADE_IN_MSEC = 1000;
        const int FADE_OUT_MSEC = 500;
        int fade_start_tc;

        private void timer1_Tick(object sender, EventArgs e)
        {
            int tc = Environment.TickCount - fade_start_tc;
            if (tc > FADE_IN_MSEC)
                timer1.Enabled = false;
            else
                this.Opacity = DEF_OPACITY*tc/FADE_IN_MSEC;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            int tc = Environment.TickCount - fade_start_tc;
            if (tc > FADE_OUT_MSEC) {
                timer2.Enabled = false;
                this.Hide();
            } else
                this.Opacity = DEF_OPACITY - DEF_OPACITY*tc/FADE_OUT_MSEC;
        }

        private void AboutForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing) {
                this.Visible = false;
                e.Cancel = true;
            }
        }

        private void AboutForm_KeyUp(object sender, KeyEventArgs e)
        {
            //this.Close();
        }

        private void AboutForm_MouseUp(object sender, MouseEventArgs e)
        {
            FadeOut();
        }

        private void AboutForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (!timer1.Enabled)
                FadeOut();
        }

        private void AboutForm_Deactivate(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
        }
    }
}
