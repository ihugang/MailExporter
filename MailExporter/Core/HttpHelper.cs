using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms.VisualStyles;
using MailExporter.Models.Input;
using MailExporter.Models.Output;
using Newtonsoft.Json;
using Serilog;


namespace MailExporter.Core
{
    public static class HttpHelper
    {
        private static readonly string DefaultUserAgent =
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.61 Safari/537.36";

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain,
            SslPolicyErrors errors)
        {
            return true; //总是接受   
        }

        /// <summary>
        /// HttpPost
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="responseContent"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool HttpPost(string url, string postData, out string responseContent, out string message)
        {
            responseContent = "";
            message = "";
            try
            {
                ServicePointManager.ServerCertificateValidationCallback =
                    CheckValidationResult;
                var request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
                request.UserAgent = DefaultUserAgent;

                var data = Encoding.UTF8.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                responseContent = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }




        public static bool Report(string appVersion, out Guid token, out string memberNo,
            out string newVersion, out string downloadUrl, out string errorMessage)
        {
            token = Guid.Empty;
            memberNo = string.Empty;
            newVersion = string.Empty;
            downloadUrl= string.Empty;
            errorMessage = string.Empty;

            try
            {
                var di = new DeviceInfo
                {
                    WechatSize = 0,
                    WechatVersion = "",
                    PhotoNum = 0,
                    VideoNum = 0,
                    DocNum = 0,
                    AppVersion = appVersion
                };

                var data = JsonConvert.SerializeObject(di);
                var url = "https://1soft.laji365.net/api/api/Device/R2";
                if (HttpPost(url, data, out var response, out var message))
                {
                    var rr = JsonConvert.DeserializeObject<InvokeResult<ReportResult>>(response);
                    token = rr.Data.Token;
                    memberNo = rr.Data.MemberNo;
                    errorMessage = rr.ErrorMessage;
                    newVersion = rr.Data.NewestVersion;
                    downloadUrl = rr.Data.DownloadUrl;
                    return true;
                }
                else
                {
                    errorMessage = message;
                    return false;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                Log.Error("上报信息时异常：" + ex.ToString());
                return false;
            }

        }

        public static bool ReportException(Guid tokenId, string message, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {

                var d = new { Message = message, Token = tokenId };
                var data = JsonConvert.SerializeObject(d);
                var url = "https://1soft.laji365.net/api/api/el/R";
                if (HttpPost(url, data, out string response, out string error))
                {
                    var rr = JsonConvert.DeserializeObject<InvokeResult>(response);
                    if (rr.success)
                    {
                        return true;
                    }
                    else
                    {
                        errorMessage = rr.errorMessage;
                        return false;
                    }
                }
                else
                {
                    errorMessage = error;
                    return false;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                Log.Error("上报异常时异常：" + ex.ToString());
                return false;
            }
        }
    }
}
