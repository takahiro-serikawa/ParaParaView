using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Drawing;

namespace ParaParaView
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);

            Application.Run(new ParaParaMain());
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            ParaParaMain.DebugOut(Color.Fuchsia, "app: {0}", e.Exception.Message);
            MessageBox.Show(e.Exception.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
