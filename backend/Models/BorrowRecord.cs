using System;  

namespace backend.Models
{
     public class BorrowRecord
    {
        // 主键，自增
        public int BorrowRecordId { get; set; }

        // 读者ID
        public string ReaderId { get; set; }

        // 图书ID
        public string BookId { get; set; }

        // 借阅时间
        public DateTime BorrowTime { get; set; }

        // 归还时间
        public DateTime? ReturnTime { get; set; }

        // 超期罚款
        public decimal OverdueFine { get; set; }
    }
}