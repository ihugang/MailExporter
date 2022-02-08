using Newtonsoft.Json;

namespace MailExporter.Models.Output
{
    public class InvokeResult
    {
        /// <summary>
        /// 成功失败
        /// </summary>
        [JsonProperty("success")]
        public bool success { get; set; }

        /// <summary>
        /// 错误提示
        /// </summary>
        [JsonProperty("errorMessage")]
        public string errorMessage { get; set; }

        [JsonProperty("errorNumber")]
        public int errorNumber { get; set; }
        
        public InvokeResult()
        {
            success = false;
            errorMessage = "";
            errorNumber = 0;
        }

    }

    public class InvokeResult<T>
    {
        /// <summary>
        /// 带数据的返回
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 成功失败
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 错误提示
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public int ErrorNumber { get; set; }
    }
}
