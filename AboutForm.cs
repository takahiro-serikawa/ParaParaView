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
    public partial class AboutForm: OverlayForm
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AboutForm(Form form) : base(form)
        {
            InitializeComponent();

            LoadLicense();

            foreach (var p in Params) {
                var elem = webBrowser1.Document.GetElementById(p.Key);
                if (elem != null)
                    elem.InnerHtml = p.Value;
            }
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

    }
}
