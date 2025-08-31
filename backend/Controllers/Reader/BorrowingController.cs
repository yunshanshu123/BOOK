using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Services.BorrowingService;
using backend.DTOs;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BorrowingController : ControllerBase
    {
        private readonly BorrowingService _borrowingService;

        /**
         * 构造函数
         * @param borrowingService BorrowingService 实例
         * @return 无
         */
        public BorrowingController(BorrowingService borrowingService)
        {
            _borrowingService = borrowingService;
        }

        /**
         * 获取所有借阅记录
         * @return 借阅记录列表
         */
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BorrowRecord>>> GetAllBorrowRecords()
        {
            var borrowRecords = await _borrowingService.GetAllBorrowRecordsAsync();
            return Ok(borrowRecords);
        }

        /**
         * 通过借阅记录ID获取借阅记录
         * @param id 借阅记录ID
         * @return 借阅记录详情
         */
        [HttpGet("{id}")]
        public async Task<ActionResult<BorrowRecord>> GetBorrowRecordByID(int id)
        {
            var borrowRecord = await _borrowingService.GetBorrowRecordByIDAsync(id);
            if (borrowRecord == null) return NotFound();
            return Ok(borrowRecord);
        }

        /**
         * 通过读者ID获取该读者的所有借阅记录
         * @param readerID 读者ID
         * @return 该读者的借阅记录列表
         */
        [HttpGet("reader/{readerID}")]
        public async Task<ActionResult<IEnumerable<BorrowRecord>>> GetBorrowRecordsByReaderID(string readerID)
        {
            var borrowRecords = await _borrowingService.GetBorrowRecordsByReaderIDAsync(readerID);
            return Ok(borrowRecords);
        }

        /**
         * 通过图书ID获取所有借阅该图书的记录
         * @param bookID 图书ID
         * @return 借阅该图书的记录列表
         */
        [HttpGet("book/{bookID}")]
        public async Task<ActionResult<IEnumerable<BorrowRecord>>> GetBorrowRecordsByBookID(string bookID)
        {
            var borrowRecords = await _borrowingService.GetBorrowRecordsByBookIDAsync(bookID);
            return Ok(borrowRecords);
        }

        /**
         * 添加新的借阅记录
         * @param dto BorrowRecordDto
         * @return 业务处理结果
         */
        [HttpPost]
        public async Task<ActionResult> AddBorrowRecord([FromBody] BorrowRecordDto dto)
        {
            if (dto == null)
            {
                return BadRequest("输入数据不能为空");
            }

            var borrowRecord = new BorrowRecord
            {
                //BorrowRecordId = dto.BorrowRecordId, // 不应该手动设置自增主键
                BookId = dto.BookId,
                ReaderId = dto.ReaderId,
                BorrowTime = dto.BorrowTime,
                ReturnTime = dto.ReturnTime,
                OverdueFine = dto.OverdueFine,
            };

            try
            {
                var result = await _borrowingService.AddBorrowRecordAsync(borrowRecord);
                return result > 0 ? Ok("借阅记录添加成功") : BadRequest("借阅记录添加失败");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"服务器错误: {ex.Message}");
            }
        }

        /**
         * 更新借阅记录
         * @param dto BorrowRecordDto
         * @return 业务处理结果
         */
        [HttpPut]
        public async Task<ActionResult> UpdateBorrowRecord([FromBody] BorrowRecordDto dto)
        {
            if (dto == null)
            {
                return BadRequest("输入数据不能为空");
            }

            var borrowRecord = new BorrowRecord
            {
                //BorrowRecordId = dto.BorrowRecordId,
                BookId = dto.BookId,
                ReaderId = dto.ReaderId,
                BorrowTime = dto.BorrowTime,
                ReturnTime = dto.ReturnTime,
                OverdueFine = dto.OverdueFine,
            };

            try
            {
                var result = await _borrowingService.UpdateBorrowRecordAsync(borrowRecord);
                return result > 0 ? Ok("借阅记录更新成功") : NotFound("借阅记录未找到");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"服务器错误: {ex.Message}");
            }
        }


        [HttpPost("return")]
        public async Task<IActionResult> ReturnBook([FromQuery] string readerId, [FromQuery] string bookId)
        {
            var response = await _borrowingService.ReturnBookAsync(readerId, bookId);

            if (response.Success)
            {
                // 成功返回200 OK
                return Ok(response);
            }
            else if (response.Message == "记录不存在")
            {
                // 记录不存在返回400 Bad Request
                return BadRequest(response);
            }
            else if (response.Message == "重复归还")
            {
                // 重复归还返回400 Bad Request
                return BadRequest(response);
            }
            else
            {
                // 其他错误返回400 Bad Request
                return BadRequest(response);
            }
        }

        /**
         * 借阅图书接口
         * @param readerId 读者ID
         * @param bookId 图书ID
         * @return 借阅操作结果
         */
        [HttpPost("borrow")]  // 与前端请求路径匹配，使用POST方法
        public async Task<IActionResult> BorrowBook([FromQuery] string readerId, [FromQuery] string bookId)
        {
            // 调用服务层的借阅方法
            var response = await _borrowingService.BorrowBookAsync(readerId, bookId);

            if (response.Success)
            {
                // 成功返回200 OK
                return Ok(response);
            }
            else
            {
                // 失败返回400 Bad Request并包含错误信息
                return BadRequest(response);
            }
        }

        /**
         * 通过借阅记录ID删除借阅记录
         * @param id 借阅记录ID
         * @return 业务处理结果
         */
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBorrowRecord(string id)
        {
            try
            {
                var result = await _borrowingService.DeleteBorrowRecordAsync(id);
                return result > 0 ? Ok("借阅记录删除成功") : NotFound("借阅记录未找到");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"服务器错误: {ex.Message}");
            }
        }
        
        /**
         * 通过读者ID查询所有借阅记录(读者自己查询)
         * @param readerid 当前读者ID
         * @return 业务处理结果
         */
        [HttpGet("reader/MyBorrowRecords/{readerId}")]
        public async Task<ActionResult> GetByReaderIdAsDto(string readerId)
        {
            var response = await _borrowingService.GetMyBorrowRecordDtosByReaderIdAsync(readerId);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }
    }
}
