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
    public partial class SlideShowDialog: Form
    {
        public SlideShowDialog()
        {
            InitializeComponent();

            Localizer.Current.Apply(null, this);
        }

        int _interval = 2500;

        public int Interval
        {
            get { return (_interval>0) ? _interval : 1; }
            set
            {
                _interval = value;
                SlideShowIntervalText.Text = string.Format("{0:F3}", _interval/1000f);
            }
        }

        public bool Shuffle
        {
            get { return ShuffleCheck.Checked; }
            set { ShuffleCheck.Checked = value; }
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            float v;
            if (float.TryParse(SlideShowIntervalText.Text, out v)) {
            //if (float.TryParse(SlideShowIntervalText.Text, out float v)) {
                _interval = (int)(v*1000);
                SlideShowIntervalText.ForeColor = Color.Black;
            } else {
                SlideShowIntervalText.ForeColor = Color.Red;
            }
        }
    }
}
