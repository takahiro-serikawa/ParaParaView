using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ParaParaView
{
    public partial class HelpForm: ParaParaView.OverlayForm
    {
        public HelpForm(Form form) : base(form)
        {
            InitializeComponent();

            LoadLicense();
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
    }
}
