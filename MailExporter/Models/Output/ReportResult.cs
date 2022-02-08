using System;

namespace MailExporter.Models.Output
{
    internal class ReportResult
    {
        /// <summary>
        /// 文档排名
        /// </summary>
        public int RankNo { get; set; }
        
        /// <summary>
        /// 文档排名信息
        /// </summary>
        public string RankInfo { get; set; }

        /// <summary>
        /// 空间排名
        /// </summary>
        public int SpaceRankNo { get; set; }

        /// <summary>
        /// 空间排名信息
        /// </summary>
        public string SpaceRankInfo { get; set; }

        /// <summary>
        /// 令牌
        /// </summary>
        public Guid Token { get; set; }

        public string MemberNo { get; set; }

        public string NewestVersion { get; set; }

        public string DownloadUrl { get; set; }
    }
}
