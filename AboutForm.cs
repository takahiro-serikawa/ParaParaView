using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ParaParaView
{
    public partial class AboutForm: ParaParaView.OverlayForm
    {
        public AboutForm(Form form) : base(form)
        {
            InitializeComponent();
            //real.PreviewKeyDown += HelpForm_PreviewKeyDown;
            real.KeyDown += HelpForm_KeyDown;
            real.KeyUp += HelpForm_KeyUp;

            LoadLicense();

        }

        public Dictionary<string, string> Params = new Dictionary<string, string>();

        private void AboutForm_Shown(object sender, EventArgs e)
        {
            foreach (var p in Params) {
                var elem = webBrowser1.Document.GetElementById(p.Key);
                if (elem != null)
                    elem.InnerHtml = p.Value;
            }
        }

        void LoadLicense()
        {
            string name = this.GetType().Namespace+@".help.html";
            var asm = System.Reflection.Assembly.GetExecutingAssembly();
            var stream = asm.GetManifestResourceStream(name);
            if (stream != null) {
                using (var sr = new System.IO.StreamReader(stream, Encoding.UTF8))
                    webBrowser1.DocumentText = sr.ReadToEnd();
                stream.Dispose();
            }
        }

        private void HelpForm_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine("KeyDown({0})", e.KeyCode.ToString());
            var result = webBrowser1.Document.InvokeScript("prekey", new string[] { "down", e.KeyCode.ToString() });
        }

        private void HelpForm_KeyUp(object sender, KeyEventArgs e)
        {
            Console.WriteLine("KeyUp({0})", e.KeyCode.ToString());
            var result = webBrowser1.Document.InvokeScript("prekey", new string[] { "up", e.KeyCode.ToString() });
        }

    }
}
