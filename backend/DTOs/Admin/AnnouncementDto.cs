using System;
using System.Collections.Generic;

namespace backend.DTOs.Admin
{
    // 用于向管理端前端展示公告列表
    public class AnnouncementDto
    {
        public int AnnouncementID { get; set; }
        public int LibrarianID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreateTime { get; set; }
        public string TargetGroup { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
    }

    // 用于创建或更新公告
    public class UpsertAnnouncementDto
    {
        public string Title { get; set; } = string.Empty; // <-- 添加默认值
        public string Content { get; set; } = string.Empty; // <-- 添加默认值
        public string TargetGroup { get; set; } = "所有人";
        public string Priority { get; set; } = "常规"; // <-- 添加默认值
    }

    // 用于公开展示的公告
    public class PublicAnnouncementsDto
    {
        public IEnumerable<AnnouncementDto> Urgent { get; set; } = new List<AnnouncementDto>();
        public IEnumerable<AnnouncementDto> Regular { get; set; } = new List<AnnouncementDto>();
    }
}