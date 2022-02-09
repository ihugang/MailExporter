namespace MailExporter
{
    partial class FrmWechat
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
            if (disposing && (components != null))
            {
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
            this.radPictureBox1 = new Telerik.WinControls.UI.RadPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.radPictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radPictureBox1
            // 
            this.radPictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPictureBox1.Image = global::MailExporter.Properties.Resources._2460DB8E_D5F6_461A_B5CA_E1A13AA19AF4_L0_001;
            this.radPictureBox1.Location = new System.Drawing.Point(0, 0);
            this.radPictureBox1.Name = "radPictureBox1";
            this.radPictureBox1.Size = new System.Drawing.Size(268, 299);
            this.radPictureBox1.TabIndex = 0;
            this.radPictureBox1.ThemeName = "ControlDefault";
            this.radPictureBox1.Click += new System.EventHandler(this.radPictureBox1_Click);
            // 
            // FrmWechat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(268, 299);
            this.Controls.Add(this.radPictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmWechat";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "技术支持微信";
            this.ThemeName = "ControlDefault";
            this.Load += new System.EventHandler(this.FrmWechat_Load);
            this.Click += new System.EventHandler(this.FrmWechat_Click);
            ((System.ComponentModel.ISupportInitialize)(this.radPictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPictureBox radPictureBox1;
    }
}
