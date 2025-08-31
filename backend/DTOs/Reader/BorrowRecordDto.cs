using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs
{
    public class BorrowRecordDto
    {   
        public string ReaderId { get; set; }

        public string BookId { get; set; }

        public string ReaderName { get; set; }

        public string BookName { get; set; }

        public DateTime BorrowTime { get; set; }

        public DateTime? ReturnTime { get; set; }

        public decimal OverdueFine { get; set; }
    }
}