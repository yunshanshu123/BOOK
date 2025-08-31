using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Admin
{
    public class HandleReportDto
    {
        [Required]
        [RegularExpression("^(approve|reject)$", ErrorMessage = "Action must be 'approve' or 'reject'.")]
        public string Action { get; set; } = string.Empty;

        // 添加这个属性，让前端告诉我们正在处理哪个评论
        [Required]
        public int CommentId { get; set; } 
    }
}