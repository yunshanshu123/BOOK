using Dapper;
using Oracle.ManagedDataAccess.Client;
using backend.Models;
using backend.DTOs;

namespace backend.Repositories.BorrowRecordRepository;

public class BorrowRecordRepository
{
    /**
     * 数据库连接字符串
     */
    private readonly string _connectionString;

    /**
     * 构造函数
     * @param connectionString 数据库连接字符串
     * @return 无
     */
    public BorrowRecordRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    /**
     * 根据 BorrowRecordID 获取 BorrowRecord 信息
     * @param borrowRecordID 借阅记录 ID
     * @return 返回 BorrowRecord 对象
     */
    public async Task<BorrowRecord> GetByIDAsync(int borrowRecordID)
    {
        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();
        var sql = "SELECT * FROM BorrowRecord WHERE BorrowRecordID = :BorrowRecordID";
        return await connection.QueryFirstOrDefaultAsync<BorrowRecord>(sql, new { BorrowRecordID = borrowRecordID });
    }

    /**
     * 根据 ReaderID 获取 BorrowRecord 信息
     * @param readerID 读者 ID
     * @return 返回 BorrowRecord 对象列表
     */
    public async Task<IEnumerable<BorrowRecord>> GetByReaderIDAsync(string readerID)
    {
        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();
        var sql = "SELECT * FROM BorrowRecord WHERE ReaderID = :ReaderID";
        return await connection.QueryAsync<BorrowRecord>(sql, new { ReaderID = readerID });
    }

    /**
     * 根据 BookID 获取 BorrowRecord 信息
     * @param bookID 图书 ID
     * @return 返回 BorrowRecord 对象列表
     */
    public async Task<IEnumerable<BorrowRecord>> GetByBookIDAsync(string bookID)
    {
        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();
        var sql = "SELECT * FROM BorrowRecord WHERE BookID = :BookID";
        return await connection.QueryAsync<BorrowRecord>(sql, new { BookID = bookID });
    }

    /**
     * 获取所有 BorrowRecord 信息
     * @return 返回 BorrowRecord 对象列表
     */
    public async Task<IEnumerable<BorrowRecord>> GetAllAsync()
    {
        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();
        var sql = "SELECT * FROM BorrowRecord";
        return await connection.QueryAsync<BorrowRecord>(sql);
    }

    /**
     * 新增一个 BorrowRecord
     * @param borrowRecord BorrowRecord 对象
     * @return 新增成功返回 1，否则返回 0
     */
    public async Task<int> AddAsync(BorrowRecord borrowRecord)
    {
        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();

        var sql = @"
            INSERT INTO BorrowRecord ( ReaderID, BookID, BorrowTime, ReturnTime, OverdueFine)
            VALUES ( :ReaderID, :BookID, :BorrowTime, :ReturnTime, : OverdueFine)";

        return await connection.ExecuteAsync(sql, borrowRecord);
    }

    /**
     * 更新一个 BorrowRecord
     * @param borrowRecord BorrowRecord 对象
     * @return 更新成功返回 1，否则返回 0
     */
    public async Task<int> UpdateAsync(BorrowRecord borrowRecord)
    {
        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();

        var sql = @"
            UPDATE BorrowRecord
            SET ReaderID = :ReaderID,
                BookID = :BookID,
                BorrowTime = :BorrowTime,
                ReturnTime = :ReturnTime,
                OverdueFine = :OverdueFine
            WHERE BorrowRecordID = :BorrowRecordID";

        return await connection.ExecuteAsync(sql, borrowRecord);
    }


    /**
     * 归还图书
     * 逻辑：通过ReaderId和BookId找到对应的借阅记录
     * - 如果已有归还时间，说明重复归还
     * - 如果没有归还时间，将当前时间设置为归还时间
     * @param readerId 读者ID
     * @param bookId 图书ID
     * @return 返回操作结果：1-成功归还，0-重复归还，-1-未找到记录
     */
    public async Task<int> ReturnBookAsync(string readerId, string bookId)
    {
        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();

        // 1. 先查询对应的借阅记录（通常是未归还的记录）
        var sqlSelect = @"
            SELECT * FROM BorrowRecord 
            WHERE ReaderID = :ReaderId 
              AND BookID = :BookId 
              AND ReturnTime IS NULL"; // 只找未归还的记录

        var borrowRecord = await connection.QueryFirstOrDefaultAsync<BorrowRecord>(
            sqlSelect,
            new { ReaderId = readerId, BookId = bookId }
        );

        // 2. 判断记录是否存在
        if (borrowRecord == null)
        {
            // 检查是否是已归还的记录（用于提示重复归还）
            var existingRecord = await connection.QueryFirstOrDefaultAsync<BorrowRecord>(
                @"SELECT * FROM BorrowRecord WHERE ReaderID = :ReaderId AND BookID = :BookId",
                new { ReaderId = readerId, BookId = bookId }
            );

            return existingRecord != null ? 0 : -1; // 0-重复归还，-1-无此记录
        }

        // 3. 执行归还操作（更新归还时间为当前时间）
        var sqlUpdate = @"
            UPDATE BorrowRecord 
            SET ReturnTime = SYSDATE  -- 使用Oracle数据库的当前时间
            WHERE BorrowRecordID = :BorrowRecordID";

        await connection.ExecuteAsync(
            sqlUpdate,
            new { BorrowRecordID = borrowRecord.BorrowRecordId }
        );

        return 1; // 归还成功
    }

    /**
     * 根据 BorrowRecordID 删除一个 BorrowRecord
     * @param borrowRecordID BorrowRecordID
     * @return 删除成功返回 1，否则返回 0
     */
    public async Task<int> DeleteAsync(string borrowRecordID)
    {
        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();
        var sql = "DELETE FROM BorrowRecord WHERE BorrowRecordID = :BorrowRecordID";
        return await connection.ExecuteAsync(sql, new { BorrowRecordID = borrowRecordID });
    }


    /**
     * 根据 readerId 和 bookId  获取 BorrowRecord 信息
     * @param borrowRecordID 借阅记录 ID
     * @return 返回 BorrowRecord 对象
     */
    public async Task<BorrowRecord> GetByReaderAndBookAsync(string readerId, string bookId)
    {
        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();
        var sql = "SELECT * FROM BorrowRecord WHERE ReaderID = :ReaderID AND BookID = :BookID";
        return await connection.QueryFirstOrDefaultAsync<BorrowRecord>(sql, new { ReaderID = readerId, BookID = bookId });
    }
    
    /**
     * 根据 readerId 该读者所有借阅信息
     * @param  readerId 读者ID
     * @return 返回 MyBorrowRecordDto 对象
     */
    public async Task<List<MyBorrowRecordDto>> GetMyBorrowRecordDtosByReaderIdAsync(string readerId)
    {
        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();

        // 联合查询借阅记录和图书信息，获取所需字段
        var sql = @"
                SELECT 
                    bi.ISBN,
                    bi.Title,
                    bi.Author,
                    br.BorrowTime,
                    br.ReturnTime,
                    br.OverdueFine
                FROM BorrowRecord br
                INNER JOIN Book b ON br.BookId = b.BookId
                INNER JOIN BOOKINFO bi ON bi.ISBN=b.ISBN
                WHERE br.ReaderId = :ReaderId
                ORDER BY br.BorrowTime DESC";

        return (await connection.QueryAsync<MyBorrowRecordDto>(
            sql,
            new { ReaderId = readerId }
        )).AsList();
    }
}
