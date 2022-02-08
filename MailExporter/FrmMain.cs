using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MailExporter.Core;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Proxy;
using MimeKit;
using Serilog;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Excel = Microsoft.Office.Interop.Excel;

namespace MailExporter
{
    public partial class FrmMain : Telerik.WinControls.UI.RadForm
    {
        private bool _runFlag = false;
        private bool _stopFlag = false;
        private string _excelFilename = string.Empty;
        private readonly List<MailboxSetting> _mailboxSettings = new List<MailboxSetting>();

        private string _imapServer;
        private string _imapProxyServer;
        private int _imapProxyPort;
        private string _imapAccount;
        private string _imapPassword;
        private int _imapPort = 993;
        private int _totalNum = 0;
        private int _recordNum = 0;
        private int _unitNum = 100;
        private Guid _token = Guid.Empty;
        private bool _downloadAttachments = false;

        private string _downloadNewVersionUrl = string.Empty;

        private string _iniFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{Program.AppName}.ini");
        private string _root = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "我的邮件");
        public FrmMain()
        {
            InitializeComponent();
        }


        #region 自定义方法
        private void InitMailboxSettings()
        {
            var mailbox = new MailboxSetting()
            {
                ImapPort = 993,
                ImapServer = "imap.qq.com",
                MailboxType = EnumMailboxType.QQ邮箱,
                Domains = new List<string>() { "qq.com" }
            };
            _mailboxSettings.Add(mailbox);

            mailbox = new MailboxSetting()
            {
                ImapPort = 993,
                ImapServer = "imap.exmail.qq.com",
                MailboxType = EnumMailboxType.腾讯企业邮箱,
                Domains = new List<string>()
            };
            _mailboxSettings.Add(mailbox);

            mailbox = new MailboxSetting()
            {
                ImapPort = 993,
                ImapServer = "hwimap.exmail.qq.com",
                MailboxType = EnumMailboxType.腾讯企业邮箱_海外用户,
                Domains = new List<string>()
            };
            _mailboxSettings.Add(mailbox);

            mailbox = new MailboxSetting()
            {
                ImapPort = 993,
                ImapServer = "imap.[domain]",
                MailboxType = EnumMailboxType.网易163免费邮箱,
                Domains = new List<string>() { "163.com", "126.com", "yeah.net" }
            };
            _mailboxSettings.Add(mailbox);

            mailbox = new MailboxSetting()
            {
                ImapPort = 993,
                ImapServer = "imap.gmail.com",
                MailboxType = EnumMailboxType.Gmail,
                Domains = new List<string>() { "gmail.com" }
            };
            _mailboxSettings.Add(mailbox);

            mailbox = new MailboxSetting()
            {
                ImapPort = 993,
                ImapServer = "outlook.office365.com",
                MailboxType = EnumMailboxType.Hotmail系列邮箱,
                Domains = new List<string>() { "hotmail.com", "msn.com", "live.com", "outlook.com" }
            };
            _mailboxSettings.Add(mailbox);

            ddlMailboxType.Items.Clear();
            foreach (var mb in _mailboxSettings)
            {
                ddlMailboxType.Items.Add(mb.MailboxType.ToString());
            }

            ddlMailboxType.Items.Add("其它");
        }

        private void ParseMailbox()
        {
            var mailbox = txtEmail.Text.Trim().ToLower();
            var blnFound = false;
            foreach (var mailboxSetting in _mailboxSettings)
            {
                if (mailboxSetting.Domains.Any(x => mailbox.EndsWith($"@{x}")))
                {
                    if (mailboxSetting.MailboxType == EnumMailboxType.网易163免费邮箱)
                    {
                        var names = mailbox.Split('@');
                        if (Name.Length == 2)
                        {
                            var domain = names[1];
                            ddlMailboxType.SelectedIndex = ddlMailboxType.FindString(mailboxSetting.MailboxType.ToString());
                            txtImapServer.Text = $"imap.{domain}";
                            txtImapPort.Text = mailboxSetting.ImapPort.ToString();
                            blnFound = true;

                            break;
                        }
                    }
                    else
                    {
                        var domain = mailboxSetting.Domains.SingleOrDefault(x => mailbox.EndsWith($"@{x}"));
                        ddlMailboxType.SelectedIndex = ddlMailboxType.FindString(mailboxSetting.MailboxType.ToString());
                        txtImapServer.Text = mailboxSetting.ImapServer;
                        txtImapPort.Text = mailboxSetting.ImapPort.ToString();
                        blnFound = true;
                        break;
                    }
                }
            }

            if (!blnFound)
            {
                var names = mailbox.Split('@');
                if (Name.Length == 2)
                {
                    var domain = names[1];
                    ddlMailboxType.SelectedIndex = ddlMailboxType.FindString("其它");
                    txtImapServer.Text = $"imap.{domain}";
                    txtImapPort.Text = "993";
                }
            }
        }

        private void SetDownloadCount(int total)
        {
            if (this.txtAttachmentNum.InvokeRequired)
            {
                this.txtAttachmentNum.BeginInvoke((MethodInvoker)delegate () { this.txtAttachmentNum.Text = total.ToString(); });
            }
            else
            {
                this.txtAttachmentNum.Text = total.ToString();
            }
        }


        /// <summary>
        ///     Write Log
        /// </summary>
        /// <param name="text"></param>
        private void WriteHistory(string text)
        {

            if (txtHistory.InvokeRequired)
            {
                Action safeWrite = delegate { WriteHistory($"{text}"); };
                txtHistory.Invoke(safeWrite);
            }
            else
            {
                txtHistory.AppendText(text + Environment.NewLine);
            }
            Log.Information(text);
            Console.WriteLine(text);

        }

        private void UpdateTotal(int total)
        {
            if (this.txtTotalNum.InvokeRequired)
            {
                this.txtTotalNum.BeginInvoke((MethodInvoker)delegate ()
                {
                    this.txtTotalNum.Text = total.ToString();
                    this.progressBarMain.Maximum = total;
                });
            }
            else
            {
                this.txtTotalNum.Text = total.ToString();
                this.progressBarMain.Maximum = total;
            }
        }

        private void SetProgress(int value)
        {
            if (this.progressBarMain.InvokeRequired)
            {
                this.progressBarMain.BeginInvoke((MethodInvoker)delegate ()
                {
                    if (value > this.progressBarMain.Maximum)
                    {
                        value = this.progressBarMain.Maximum;
                    }
                    this.progressBarMain.Value1 = value;

                    this.progressBarMain.Text = ((value * 100) / this.progressBarMain.Maximum).ToString() + "%";
                    this.txtExportNum.Text = value.ToString();
                });
            }
            else
            {
                if (value > this.progressBarMain.Maximum)
                {
                    value = this.progressBarMain.Maximum;
                }
                this.progressBarMain.Value1 = value;
                this.txtExportNum.Text = value.ToString();
                this.progressBarMain.Text = ((value * 100) / this.progressBarMain.Maximum).ToString() + "%";
            }
        }

        private void ResetButtons()
        {
            if (this.btnStart.InvokeRequired)
            {
                this.btnStart.BeginInvoke((MethodInvoker)delegate ()
                {
                    lblInfo.Text = Program.AppSlogan;
                    btnStart.Enabled = true;
                    btnPause.Enabled = false;
                    this.Cursor = Cursors.Default;
                });
            }
            else
            {
                lblInfo.Text = Program.AppSlogan;
                btnStart.Enabled = true;
                btnPause.Enabled = false;
                this.Cursor = Cursors.Default;
            }

        }

        private void WriteInfo(string s)
        {
            if (this.lblInfo.InvokeRequired)
            {
                this.lblInfo.BeginInvoke((MethodInvoker)delegate ()
                {
                    lblInfo.Text = s;
                });
            }
            else
            {
                lblInfo.Text = s;
            }
        }

        private void SetButtons()
        {
            if (this.btnStart.InvokeRequired)
            {
                this.btnStart.BeginInvoke((MethodInvoker)delegate ()
                {
                    lblInfo.Text = "正在与邮件服务器通讯获取信息......";
                    btnStart.Enabled = false;
                    btnPause.Enabled = true;
                    this.Cursor = Cursors.WaitCursor;
                });
            }
            else
            {
                lblInfo.Text = "正在与邮件服务器通讯获取信息......";
                btnStart.Enabled = false;
                btnPause.Enabled = true;
                this.Cursor = Cursors.WaitCursor;
            }

            _stopFlag = false;

        }

        private void Report(bool runNow = false)
        {
            try
            {
                var success = HttpHelper.Report(Application.ProductVersion,
                    out var token, out var memberNo,
                    out var newVersion, out var downloadUrl,
                    out string error);
                if (!success)
                {
                    WriteHistory(error);
                }
                else
                {
                    _token = token;
                    if (new Version(Application.ProductVersion) < new Version(newVersion))
                    {
                        this.Text = $"{Program.AppTitle} 【{memberNo}】 有新版本！";
                        _downloadNewVersionUrl = downloadUrl;
                    }
                    else
                    {
                        this.Text = $"{Program.AppTitle} 【{memberNo}】";
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }


        #endregion

        #region Form事件处理


        private void FrmMain_Load(object sender, EventArgs e)
        {
            Report();
            txtTotalNum.Text = "0";
            txtExportNum.Text = "0";
            txtAttachmentNum.Text = "0";

            ddlUnitNum.SelectedIndex = 0;

            txtExcelFile.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                "我的邮件");
            txtExcelFile.IsReadOnly = true;

            _root = txtExcelFile.Text;

            if (!Directory.Exists(_root))
            {
                Directory.CreateDirectory(_root);
            }


            var server = INIHelper.Read("iMap0", "server", "", _iniFile);
            var port = INIHelper.Read("iMap0", "port", "993", _iniFile);
            var email = INIHelper.Read("iMap0", "email", "", _iniFile);
            var proxy = INIHelper.Read("iMap0", "proxy", "", _iniFile);

            txtImapPort.Text = port;
            txtImapServer.Text = server;
            txtEmail.Text = email;
            txtProxy.Text = proxy;
            InitMailboxSettings();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (_runFlag)
            {
                _stopFlag = true;
                Thread.Sleep(200);
            }

            this.Close();
        }


        #endregion

        private void btnTest_Click(object sender, EventArgs e)
        {
            btnTest.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            if (!string.IsNullOrEmpty(txtProxy.Text))
            {
                var addr = txtProxy.Text.Split(':');
                if (addr.Length == 2)
                {
                    try
                    {
                        _imapProxyServer = addr[0];
                        _imapProxyPort = int.Parse(addr[1]);
                    }
                    catch (Exception ex)
                    {
                        WriteHistory("代理设置错误：" + ex.Message);

                    }
                }
            }

            using (ImapClient client = new ImapClient())
            {
                try
                {
                    if (!string.IsNullOrEmpty(_imapProxyServer) && _imapProxyPort > 0)
                    {
                        client.ProxyClient = new Socks5Client(_imapProxyServer, _imapProxyPort);
                    }
                    client.Connect(txtImapServer.Text, int.Parse(txtImapPort.Text), true);
                    client.Authenticate(new NetworkCredential(txtEmail.Text, txtPassword.Text));
                    if (client.IsAuthenticated)
                    {
                        INIHelper.Write("iMap0", "server", txtImapServer.Text, _iniFile);
                        INIHelper.Write("iMap0", "port", txtImapPort.Text, _iniFile);
                        INIHelper.Write("iMap0", "email", txtEmail.Text, _iniFile);
                        INIHelper.Write("iMap0", "proxy", txtProxy.Text, _iniFile);

                        _imapPassword = txtPassword.Text;
                        _imapServer = txtImapServer.Text;
                        _imapPort = int.Parse(txtImapPort.Text);
                        _imapAccount = txtEmail.Text;

                        var inbox = client.Inbox;
                        inbox.Open(FolderAccess.ReadOnly);
                        txtTotalNum.Text = inbox.Count.ToString();
                        _totalNum = inbox.Count;
                        UpdateTotal(_totalNum);
                        var folder = Path.Combine(_root, txtEmail.Text);
                        if (!Directory.Exists(folder))
                        {
                            Directory.CreateDirectory(folder);
                        }

                        _excelFilename = Path.Combine(folder, "MailsArchive.xlsx");

                        txtExcelFile.Text = _excelFilename;
                        MessageBox.Show(@"登录成功！", Program.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnStart.Enabled = true;
                        btnTool.Enabled = true;
                    }
                    btnTest.Enabled = true;
                }
                catch (Exception ex)
                {
                    Log.Information(@"登录异常：" + ex.ToString());
                    this.Cursor = Cursors.Default;
                    MessageBox.Show(ex.Message, Program.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnTest.Enabled = true;
                }
            }


        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            ParseMailbox();
        }


        private void Process()
        {
            var isFinished = false;
            while (_totalNum > _recordNum && !_stopFlag)
            {
                try
                {
                    var isAppendMode = File.Exists(_excelFilename);

                    var attachmentsFolder = Path.Combine(_root, _imapAccount, "附件");

                    if (!Directory.Exists(attachmentsFolder))
                    {
                        Directory.CreateDirectory(attachmentsFolder);
                    }

                    var files = Directory.GetFiles(attachmentsFolder, "*.*");

                    var attachmentCount = files.Length;


                    DataTable dt = new DataTable();

                    if (isAppendMode)
                    {
                        dt = ExcelHelper.ExcelToTable(_excelFilename);
                    }

                    var rows = dt.Rows;
                    var excelRow = rows.Count;
                    _recordNum = excelRow;
                    SetProgress(_recordNum);

                    if (_recordNum == 0)
                    {
                        dt.Columns.Add("序号", Type.GetType("System.Int32"));
                        dt.Columns.Add("发件人", Type.GetType("System.String"));
                        dt.Columns.Add("发送日期", Type.GetType("System.String"));
                        dt.Columns.Add("标题", Type.GetType("System.String"));
                        dt.Columns.Add("附件", Type.GetType("System.Int32"));
                        dt.Columns.Add("大小", Type.GetType("System.Int32"));
                    }

                    using (ImapClient client = new ImapClient())
                    {
                        if (!string.IsNullOrEmpty(_imapProxyServer) && _imapProxyPort > 0)
                        {
                            client.ProxyClient = new Socks5Client(_imapProxyServer, _imapProxyPort);
                        }
                        client.Connect(_imapServer, _imapPort, true);
                        client.Authenticate(new NetworkCredential(_imapAccount, _imapPassword));

                        var inbox = client.Inbox;
                        inbox.Open(FolderAccess.ReadOnly);


                        int pageNum = _unitNum;
                        var pages = (inbox.Count + pageNum - 1) / pageNum;

                        var received = dt.Rows.Count;
                        if (received == 0)
                        {
                            received = 0;
                        }
                        else
                        {
                            excelRow = received + 1;
                        }

                        try
                        {

                            #region 归档邮件
                            if (inbox.Count > 0)
                            {
                                _totalNum = inbox.Count;
                                UpdateTotal(_totalNum);

                                #region 下载邮件
                                for (int i = 1; i <= pages; i++)
                                {
                                    int pageEndIndex = i * pageNum - 1;
                                    int pageStartIndex = (i - 1) * pageNum;

                                    if (received > pageEndIndex) continue;

                                    if (received > pageStartIndex && received <= pageEndIndex)
                                    {
                                        pageStartIndex = received;
                                    }

                                    if (i == pages)
                                    {
                                        pageEndIndex = inbox.Count - 1;
                                    }
                                    else if (i == pages)
                                    {
                                        if (pageEndIndex > _totalNum - 1)
                                        {
                                            pageEndIndex = _totalNum - 1;
                                        }
                                    }

                                    if (pageEndIndex < pageStartIndex) break;

                                    WriteHistory($"正在请求从{pageStartIndex}到{pageEndIndex}封邮件......");

                                    var messages = inbox.Fetch(pageStartIndex, pageEndIndex,
                                        MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure | MessageSummaryItems.Size);

                                    messages = messages
                                        .OrderBy(message => message.UniqueId.Id).ToList();

                                    foreach (var message in messages)
                                    {
                                        Console.WriteLine($"{message.NormalizedSubject}{message.Envelope.Date?.DateTime}");
                                        var row = dt.NewRow();
                                        row["序号"] = message.UniqueId.Id;
                                        if (message.Envelope.From.Any())
                                        {
                                            row["发件人"] = message.Envelope.From.First()?.ToString();
                                        }

                                        row["发送日期"] = message.Envelope.Date?.DateTime;
                                        row["标题"] = message.NormalizedSubject;
                                        row["附件"] = message.Attachments.Count();
                                        row["大小"] = message.Size;
                                        dt.Rows.Add(row);

                                        if (_downloadAttachments)
                                        {
                                            if (message.Attachments.Any())
                                            {
                                                WriteHistory($"获取{message.NormalizedSubject} 附件数据...");
                                                var messageFull = inbox.GetMessage(message.UniqueId);
                                                foreach (var attachment in messageFull.Attachments)
                                                {
                                                    var fileName = attachment.ContentDisposition?.FileName ??
                                                                   attachment.ContentType.Name;

                                                    if (fileName != null)
                                                    {
                                                        var ext = Path.GetExtension(fileName.ToLower());
                                                        var saveFilename = Path.Combine(_root, _imapAccount, "附件",
                                                            $"{message.UniqueId.Id:D6}_{fileName}");
                                                        if (File.Exists(saveFilename)) break;
                                                        ;
                                                        using (var stream = File.Create(saveFilename))
                                                        {
                                                            if (attachment is MessagePart)
                                                            {
                                                                var rfc822 = (MessagePart)attachment;

                                                                rfc822.Message.WriteTo(stream);
                                                            }
                                                            else
                                                            {
                                                                var part = (MimePart)attachment;

                                                                part.Content.DecodeTo(stream);
                                                            }
                                                        }

                                                        attachmentCount++;
                                                        WriteHistory($"{attachmentCount}: {message.NormalizedSubject} 附件：{fileName}");
                                                        File.SetCreationTime(saveFilename, message.Date.DateTime);
                                                        File.SetLastAccessTime(saveFilename, message.Date.DateTime);
                                                        File.SetLastWriteTime(saveFilename, message.Date.DateTime);
                                                        SetDownloadCount(attachmentCount);
                                                    }

                                                }
                                            }
                                        }

                                        _recordNum = excelRow;
                                        excelRow++;
                                        SetProgress(_recordNum);

                                        WriteHistory($"{_recordNum}、{message.NormalizedSubject}");
                                        Thread.Sleep(1);
                                        if (_stopFlag)
                                        {
                                            INIHelper.Write("iMap0", "received", (_recordNum).ToString(), _iniFile);
                                            break;
                                        }
                                    }


                                    if (_stopFlag)
                                    {
                                        INIHelper.Write("iMap0", "received", (_recordNum).ToString(), _iniFile);
                                        break;
                                    }
                                    Thread.Sleep(100);
                                }

                                #endregion

                                #region 检查一遍附件
                                if (_downloadAttachments && !_stopFlag && _recordNum == _totalNum)
                                {
                                    WriteInfo("正在检查附件下载进度...");
                                    var files2 = (new DirectoryInfo(attachmentsFolder)).GetFiles("*.*");
                                    var dt2 = dt.AsEnumerable();
                                    UpdateTotal(dt2.Count());
                                    _recordNum = 0;
                                    attachmentCount = files2.Length;
                                    SetDownloadCount(attachmentCount);

                                    var mailwithAttachmentsCount = dt2.Count(x => x.Field<string>("附件") != "0");
                                    progressBarMain.Maximum = mailwithAttachmentsCount;

                                    foreach (var r in dt2.Where(x => x.Field<string>("附件") != "0"))
                                    {
                                        var id = int.Parse(r.ItemArray[0].ToString());
                                        if (files2.Any(x => x.Name.StartsWith($"{id}_") || x.Name.StartsWith($"{id:D6}")))
                                        {
                                            var attachFiles = files2.Where(x =>
                                                x.Name.StartsWith($"{id}_") || x.Name.StartsWith($"{id:D6}"));
                                            var mailDate = DateTime.Parse(r.ItemArray[2].ToString());
                                            foreach (var afile in attachFiles)
                                            {
                                                if (afile.LastWriteTime.Date != mailDate.Date)
                                                {
                                                    File.SetCreationTime(afile.FullName, mailDate);
                                                    File.SetLastWriteTime(afile.FullName, mailDate);
                                                    File.SetLastAccessTime(afile.FullName, mailDate);
                                                    WriteHistory($"重置文件日期{afile.Name} {mailDate:s}");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            WriteHistory($"获取{r.ItemArray[3].ToString()}附件数据...");
                                            try
                                            {
                                                var messageFull = inbox.GetMessage(id);
                                                foreach (var attachment in messageFull.Attachments)
                                                {
                                                    var fileName = attachment.ContentDisposition?.FileName ??
                                                                   attachment.ContentType.Name;

                                                    if (fileName != null)
                                                    {
                                                        var ext = Path.GetExtension(fileName.ToLower());
                                                        var saveFilename = Path.Combine(_root, _imapAccount, "附件",
                                                            $"{id:D6}_{fileName}");
                                                        if (File.Exists(saveFilename)) break;
                                                        ;
                                                        using (var stream = File.Create(saveFilename))
                                                        {
                                                            if (attachment is MessagePart)
                                                            {
                                                                var rfc822 = (MessagePart)attachment;

                                                                rfc822.Message.WriteTo(stream);
                                                            }
                                                            else
                                                            {
                                                                var part = (MimePart)attachment;

                                                                part.Content.DecodeTo(stream);
                                                            }
                                                        }

                                                        attachmentCount++;
                                                        WriteHistory(
                                                            $"{attachmentCount}: {messageFull.Subject} 附件：{fileName}");
                                                        File.SetCreationTime(saveFilename, messageFull.Date.DateTime);
                                                        File.SetLastAccessTime(saveFilename, messageFull.Date.DateTime);
                                                        File.SetLastWriteTime(saveFilename, messageFull.Date.DateTime);
                                                        SetDownloadCount(attachmentCount);
                                                    }

                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                WriteHistory(ex.Message);
                                            }
                                        }

                                        _recordNum++;
                                        SetProgress(_recordNum);
                                        if (_stopFlag) break;

                                    }

                                    _stopFlag = true;
                                    isFinished = true;
                                }

                                #endregion
                            }
                            #endregion

                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex.ToString());
                            INIHelper.Write("iMap0", "received", (_recordNum).ToString(), _iniFile);
                            WriteHistory(ex.Message);
                        }

                    }

                    ExcelHelper.TableToExcel(dt, _excelFilename);



                    ResetButtons();
                }
                catch (Exception ex)
                {
                    Log.Information(@"导出异常：" + ex.ToString());

                    WriteHistory(ex.Message);
                    ResetButtons();
                }
            }

            WriteHistory(_totalNum > _recordNum && isFinished ? "导出操作停止！" : "导出完成。");
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            SetButtons();
            _downloadAttachments = cbDownloadAttachments.Checked;
            _stopFlag = false;
            _unitNum = int.Parse(ddlUnitNum.SelectedItem.ToString());

            _imapAccount = txtEmail.Text.Trim();
            _imapPassword = txtPassword.Text.Trim();
            _imapServer = txtImapServer.Text.Trim();
            _imapPort = int.Parse(txtImapPort.Text);
            WriteHistory("开始导出邮件，请稍候......");
            _recordNum = 0;
            var thread = new Thread(Process);
            thread.Start();
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            _stopFlag = true;
            WriteHistory("正在结束本次操作，请等候......");
        }

        private void txtImapPort_TextChanged(object sender, EventArgs e)
        {
            var newText = txtImapPort.Text.Trim();
            if (int.TryParse(newText, out int newPort) && newPort > 0)
            {
                _imapPort = newPort;
            }
            else
            {
                MessageBox.Show("服务器端口信息不正确！", Program.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ddlMailboxType_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            var select = ddlMailboxType.SelectedItem.ToString();
            var mailBoxSetting = _mailboxSettings.FirstOrDefault(x => x.MailboxType.ToString() == select);
            txtImapServer.Text = mailBoxSetting.ImapServer;
            txtImapPort.Text = mailBoxSetting.ImapPort.ToString();
        }

        private void btnTool_Click(object sender, EventArgs e)
        {
            var form = new FrmUtility();
            form.ImapServer = _imapServer;
            form.ImapPort = _imapPort;
            form.Password = _imapPassword;
            form.Username = _imapAccount;

            form.ProxyServer = _imapProxyServer;
            form.ProxyPort = _imapProxyPort;

            form.ShowDialog();
        }

        private void linkDownload_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_downloadNewVersionUrl))
            {
                System.Diagnostics.Process.Start(_downloadNewVersionUrl);
            }
        }
    }
}
