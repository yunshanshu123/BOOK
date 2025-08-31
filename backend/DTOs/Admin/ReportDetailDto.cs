using System;

namespace backend.DTOs.Admin
{
    public class ReportDetailDto
    {
        // Report Info
        public int ReportID { get; set; }
        public string ReportReason { get; set; } = string.Empty;
        public DateTime ReportTime { get; set; }
        public string ReportStatus { get; set; } = string.Empty;

        // Comment Info
        public int CommentID { get; set; }
        public string ReviewContent { get; set; } = string.Empty;
        public DateTime CommentTime { get; set; }

        // User Info
        public int CommenterID { get; set; }
        public string CommenterNickname { get; set; } = string.Empty;
        public int ReporterID { get; set; }
        public string ReporterNickname { get; set; } = string.Empty;

        // Book Info
        public string ISBN { get; set; } = string.Empty;
        public string BookTitle { get; set; } = string.Empty;
    }
}