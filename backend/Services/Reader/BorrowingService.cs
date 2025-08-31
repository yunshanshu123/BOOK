using backend.DTOs;
using backend.Models;
using backend.Repositories.BorrowRecordRepository;

namespace backend.Services.BorrowingService
{
    public class BorrowingService
    {
        private readonly BorrowRecordRepository _borrowRecordRepository;

        // 构造函数
        public BorrowingService(BorrowRecordRepository borrowRecordRepository)
        {
            _borrowRecordRepository = borrowRecordRepository;
        }

        // 异步方法，通过借阅记录ID获取借阅记录
        public async Task<BorrowRecord> GetBorrowRecordByIDAsync(int borrowRecordID)
        {
            return await _borrowRecordRepository.GetByIDAsync(borrowRecordID);
        }

        // 异步方法，通过读者ID获取该读者的所有借阅记录
        public async Task<IEnumerable<BorrowRecord>> GetBorrowRecordsByReaderIDAsync(string readerID)
        {
            return await _borrowRecordRepository.GetByReaderIDAsync(readerID);
        }

        // 异步方法，通过图书ID获取所有借阅该图书的记录
        public async Task<IEnumerable<BorrowRecord>> GetBorrowRecordsByBookIDAsync(string bookID)
        {
            return await _borrowRecordRepository.GetByBookIDAsync(bookID);
        }

        // 异步方法，获取所有的借阅记录
        public async Task<IEnumerable<BorrowRecord>> GetAllBorrowRecordsAsync()
        {
            return await _borrowRecordRepository.GetAllAsync();
        }

        // 图书借阅功能
        public async Task<BorrowingServiceResponse<string>> BorrowBookAsync(string readerId, string bookId)
        {
            // 参数验证
            if (string.IsNullOrEmpty(readerId) || string.IsNullOrEmpty(bookId))
            {
                return new BorrowingServiceResponse<string>
                {
                    Success = false,
                    Message = "读者ID和图书ID不能为空",
                    Data = null
                };
            }

            // 检查书本是否已经借阅 且未归还
            var existingRecords = await _borrowRecordRepository.GetByReaderAndBookAsync(readerId, bookId);
            if (existingRecords == null)
            {
                return new BorrowingServiceResponse<string>
                {
                    Success = false,
                    Message = "已借阅未归还",
                    Data = $"读者者 {readerId} 已借阅图书 {bookId} 且未归还"
                };
            }

            // 创建新的借阅记录，自动设置当前时间为借阅时间
            var newRecord = new BorrowRecord
            {
                ReaderId = readerId,
                BookId = bookId,
                BorrowTime = DateTime.Now,  // 自动填充当前时间
                ReturnTime = null,          // 未归还
                OverdueFine = 0             // 初始逾期费用为0
            };

            // 保存到数据库
            var result = await _borrowRecordRepository.AddAsync(newRecord);

            if (result > 0)
            {
                return new BorrowingServiceResponse<string>
                {
                    Success = true,
                    Message = "图书借阅成功",
                    Data = $"读者 {readerId} 成功借阅图书 {bookId}，借阅时间：{DateTime.Now:yyyy-MMMMdd HH:mm:ss}"
                };
            }
            else
            {
                return new BorrowingServiceResponse<string>
                {
                    Success = false,
                    Message = "借阅失败",
                    Data = "添加新借阅记录时发生错误"
                };
            }
        }

        // 异步方法，归还图书
        public async Task<BorrowingServiceResponse<string>> ReturnBookAsync(string readerId, string bookId)
        {
            // 参数验证
            if (string.IsNullOrEmpty(readerId) || string.IsNullOrEmpty(bookId))
            {
                return new BorrowingServiceResponse<string>
                {
                    Success = false,
                    Message = "读者ID和图书ID不能为空",
                    Data = null
                };
            }

            // 调用仓库层的归还方法
            var result = await _borrowRecordRepository.ReturnBookAsync(readerId, bookId);

            // 根据仓库返回结果构建服务响应
            return result switch
            {
                1 => new BorrowingServiceResponse<string>
                {
                    Success = true,
                    Message = "图书归还成功",
                    Data = $"读者 {readerId} 已成功归还图书 {bookId}"
                },
                0 => new BorrowingServiceResponse<string>
                {
                    Success = false,
                    Message = "重复归还",
                    Data = $"读者 {readerId} 已归还过图书 {bookId}，不能重复归还"
                },
                -1 => new BorrowingServiceResponse<string>
                {
                    Success = false,
                    Message = "记录不存在",
                    Data = $"未找到读者 {readerId} 借阅图书 {bookId} 的记录"
                },
                _ => new BorrowingServiceResponse<string>
                {
                    Success = false,
                    Message = "操作失败",
                    Data = "归还图书时发生未知错误"
                }
            };
        }

        // 异步方法，添加新的借阅记录
        public async Task<int> AddBorrowRecordAsync(BorrowRecord borrowRecord)
        {
            return await _borrowRecordRepository.AddAsync(borrowRecord);
        }

        // 异步方法，更新借阅记录
        public async Task<int> UpdateBorrowRecordAsync(BorrowRecord borrowRecord)
        {
            return await _borrowRecordRepository.UpdateAsync(borrowRecord);
        }

        // 异步方法，通过借阅记录ID删除借阅记录
        public async Task<int> DeleteBorrowRecordAsync(string borrowRecordID)
        {
            return await _borrowRecordRepository.DeleteAsync(borrowRecordID);
        }
        
        // 新增：通过读者ID获取该读者的所有借阅记录并转换为DTO
        public async Task<BorrowingServiceResponse<List<MyBorrowRecordDto>>> GetMyBorrowRecordDtosByReaderIdAsync(string readerId)
        {
            if (string.IsNullOrEmpty(readerId))
            {
                return new BorrowingServiceResponse<List<MyBorrowRecordDto>>
                {
                    Success = false,
                    Message = "读者ID不能为空"
                };
            }

            try
            {
                var records = await _borrowRecordRepository.GetMyBorrowRecordDtosByReaderIdAsync(readerId);
                return new BorrowingServiceResponse<List<MyBorrowRecordDto>>
                {
                    Success = true,
                    Data = records,
                    Message = records.Any() ? "查询成功" : "未找到借阅记录"
                };
            }
            catch (Exception ex)
            {
                return new BorrowingServiceResponse<List<MyBorrowRecordDto>>
                {
                    Success = false,
                    Message = $"查询失败：{ex.Message}"
                };
            }
        }
    }
    
    // 通用服务响应模型
    public class BorrowingServiceResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}