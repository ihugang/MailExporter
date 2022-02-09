using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Serilog;

namespace MailExporter
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string logFolder = Path.Combine(Application.StartupPath, "Logs");
            if (!Directory.Exists(logFolder))
            {
                Directory.CreateDirectory(logFolder);
            }

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.File("logs//Log.txt",//文件保存路径
                    outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss}] [{Level:u3}] {Message:lj}{NewLine}{Exception}",//输出日期格式
                    rollingInterval: RollingInterval.Day,//日志按日保存
                    rollOnFileSizeLimit: true,          // 限制单个文件的最大长度   
                    encoding: Encoding.UTF8,            // 文件字符编码     
                    retainedFileCountLimit: 10,         // 最大保存文件数     
                    fileSizeLimitBytes: 10*1024 * 1024)      // 最大单个文件长度
                .CreateLogger();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());
        }

        public static string AppName = "MailExporter";
        public static string AppTitle = "电子邮件导出Excel工具";
        public static string AppSlogan = "时间猎犬系列——电子邮件导出为Excel工具 Save your time!";
        public static Guid ApplicationId = Guid.Parse("CCF5533F-B411-495D-9D09-EE71C59E9954");
    }
}