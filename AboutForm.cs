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
    public partial class AboutForm: Form
    {
        public AboutForm()
        {
            InitializeComponent();

            LoadLisence();
        }

        void LoadLisence()
        {
            //string name = this.GetType().Namespace+@".lisence.rtf";
            string name = this.GetType().Namespace+@".lisence.html";
            var asm = System.Reflection.Assembly.GetExecutingAssembly();
            var stream = asm.GetManifestResourceStream(name);
            if (stream != null) {
                using (var sr = new StreamReader(stream, Encoding.UTF8)) {
                    string lisence = sr.ReadToEnd();
                    webBrowser1.DocumentText = lisence;

                    //richTextBox1.Rtf =  lisence;
                    //Point pt = richTextBox1.GetPositionFromCharIndex(richTextBox1.Text.Length-1);
                    //richTextBox1.Height = pt.Y;
                }
                stream.Dispose();
            }
        }

        int fade_tc;

        public void FadeIn(Rectangle bounds)
        {
            this.Bounds = bounds;
            //richTextBox1.Left = bounds.Width/2;
            //webBrowser1.Left = bounds.Width/2;

            this.Opacity = 0;
            fade_tc = Environment.TickCount;
            this.Show();
            timer1.Enabled = true;
            this.Focus();
        }

        int open_tc = Environment.TickCount;

        private void timer1_Tick(object sender, EventArgs e)
        {
            int tc = Environment.TickCount - fade_tc;
            if (tc > 1000)
                timer1.Enabled = false;
            else
                this.Opacity = 0.75*tc/1000;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            int tc = Environment.TickCount;
            //richTextBox1.Top = -(tc - open_tc)/30;
            //richTextBox1.Height = 10000;
        }

        private void AboutForm_KeyUp(object sender, KeyEventArgs e)
        {
            //this.Close();
        }

        private void AboutForm_MouseUp(object sender, MouseEventArgs e)
        {
            this.Close();
        }

        private void AboutForm_KeyDown(object sender, KeyEventArgs e)
        {
            this.Close();
        }
    }
}
