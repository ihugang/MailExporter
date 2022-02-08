using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Proxy;
using MailKit.Search;
using NPOI.OpenXmlFormats.Wordprocessing;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace MailExporter
{
   
    public partial class FrmUtility : Telerik.WinControls.UI.RadForm
    {
        #region 属性

        public string ImapServer { get; set; }

        public int ImapPort { get; set; }

        public string Password { get; set; }

        public string Username { get; set; }

        public string ProxyServer { get; set; }

        public int ProxyPort { get; set; }

        private List<DataMailItem> _mails { get; set; }

        #endregion
        public FrmUtility()
        {
            InitializeComponent();
        }

        #region Form事件处理
        private void btnReturn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        private void RefreshMails()
        {
            SetInfo("正在与邮件服务器通讯，请稍候...");
            using (ImapClient client = new ImapClient())
            {

                if (!string.IsNullOrEmpty(ProxyServer) && ProxyPort > 0)
                {
                    client.ProxyClient = new Socks5Client(ProxyServer, ProxyPort);
                }

                client.Connect(ImapServer, ImapPort, true);
                client.Authenticate(new NetworkCredential(Username, Password));

                var inbox = client.Inbox;

                inbox.Open(FolderAccess.ReadOnly);
                var uids = inbox.Search(SearchQuery.SubjectContains(txtKeywords.Text).Or(SearchQuery.FromContains(txtKeywords.Text)));

                var mails = inbox.Fetch(uids, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope);

                _mails = new List<DataMailItem>();

                foreach (var mail in mails)
                {
                    var item = new DataMailItem()
                    {
                        IsSelected = true,
                        No = mail.UniqueId.Id,
                        Subject = mail.Envelope.Subject,
                        SendTime = mail.Envelope.Date?.Date
                    };

                    _mails.Add(item);
                }
                SetMailCount((uint)_mails.Count);
                SetGridDatasource(null);
                SetGridDatasource(_mails);
                SetInfo("邮箱工具");
            }
        }

        private void SetGridDatasource(object source)
        {
            if (this.viewMain.InvokeRequired)
            {
                this.viewMain.BeginInvoke((MethodInvoker) delegate() { this.viewMain.DataSource = source; });
            }
            else
            {
                this.viewMain.DataSource = source;
            }
        }

        private void SetInfo(string source)
        {
            if (this.lblCount.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate () { this.Text = source; });
            }
            else
            {
                this.Text = source;
            }
        }

        private void ClearInfo()
        {
            if (this.lblCount.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate () { this.Text = ""; });
            }
            else
            {
                this.Text = "";
            }
        }

        private void SetMailCount(uint count)
        {
            if (this.lblCount.InvokeRequired)
            {
                this.lblCount.BeginInvoke((MethodInvoker)delegate () { this.lblCount.Text = "邮件数量：" + count.ToString(); });
            }
            else
            {
                this.lblCount.Text = "邮件数量：" +  count.ToString();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtKeywords.Text))
            {
                MessageBox.Show(@"请输入关键词！", Program.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var thread = new Thread(RefreshMails);
            thread.Start();
        }

        private void FrmUtility_Load(object sender, EventArgs e)
        {
            viewMain.AutoGenerateColumns=false;
            ClearInfo();
        }

        private void viewMain_ValueChanged(object sender, EventArgs e)
        {
            if (this.viewMain.ActiveEditor is RadCheckBoxEditor)
            {
                Console.WriteLine(this.viewMain.CurrentCell.RowIndex);
                Console.WriteLine(this.viewMain.ActiveEditor.Value);
                var item = (DataMailItem) this.viewMain.CurrentCell.Value;
                item.IsSelected = !item.IsSelected;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.viewMain.Rows.Count > 0)
            {
                SetInfo("正在删除邮件，请稍候......");
                var ids = new List<UniqueId>();
                foreach (var item in viewMain.Rows)
                {
                    var data = (DataMailItem) item.DataBoundItem;
                    var id = new UniqueId(data.No);
                    ids.Add(id);
                }

                using (ImapClient client = new ImapClient())
                {

                    if (!string.IsNullOrEmpty(ProxyServer) && ProxyPort > 0)
                    {
                        client.ProxyClient = new Socks5Client(ProxyServer, ProxyPort);
                    }

                    client.Connect(ImapServer, ImapPort, true);
                    client.Authenticate(new NetworkCredential(Username, Password));

                    var inbox = client.Inbox;
                    inbox.Open(FolderAccess.ReadWrite);

                    inbox.AddFlags(ids, MessageFlags.Deleted, true);
                    inbox.Expunge();
                    SetInfo("邮箱工具");
                    MessageBox.Show("删除成功！", Program.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SetGridDatasource(null);
                    ClearInfo();
                }
            }
        }
    }
}
