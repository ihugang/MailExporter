using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace MailExporter
{
    public partial class FrmWechat : Telerik.WinControls.UI.RadForm
    {
        public FrmWechat()
        {
            InitializeComponent();
        }


        private void radPictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmWechat_Load(object sender, EventArgs e)
        {
        }

        private void FrmWechat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
