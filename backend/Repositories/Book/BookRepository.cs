﻿public class BookRepository
{
    private readonly string _connectionString;

    public BookRepository(string connectionString)
    {
        _connectionString = connectionString;
    }
    // ========== 按书名/作者搜索（只查 BookInfo表） ==========
    public async Task<IEnumerable<BookInfoDto>> SearchBooksAsync(string keyword)
    {
        var sql = @"
            SELECT ISBN, Title, Author
            FROM BookInfo
            WHERE LOWER(Title) LIKE :keyword OR LOWER(Author) LIKE :keyword";

        using var connection = new Oracle.ManagedDataAccess.Client.OracleConnection(_connectionString);
        await connection.OpenAsync();

        return await Dapper.SqlMapper.QueryAsync<BookInfoDto>(
            connection, sql, new { keyword = $"%{keyword.ToLower()}%" });
    }
    // ========== 按 BookID 状态status流转，原子操作并发安全 ==========
    public async Task<bool> UpdateStatusIfMatchesAsync(int bookId, string expectedStatus, string newStatus, CancellationToken ct = default)
    {
        const string sql = @"
            UPDATE Book
               SET Status = :newStatus
             WHERE BookID = :bookId
               AND Status = :expectedStatus";

        using var connection = new Oracle.ManagedDataAccess.Client.OracleConnection(_connectionString);
        await connection.OpenAsync(); 

        var affected = await Dapper.SqlMapper.ExecuteAsync(
            connection, sql, new { newStatus, bookId, expectedStatus });

        return affected == 1;
    }
    // ========== 按条码状态status流转（扫码借还用） ==========
    public async Task<bool> UpdateStatusIfMatchesByBarcodeAsync(string barcode, string expectedStatus, string newStatus, CancellationToken ct = default)
    {
        const string sql = @"
            UPDATE Book
               SET Status = :newStatus
             WHERE Barcode = :barcode
               AND Status  = :expectedStatus";

        using var connection = new Oracle.ManagedDataAccess.Client.OracleConnection(_connectionString);
        await connection.OpenAsync();

        var affected = await Dapper.SqlMapper.ExecuteAsync(
            connection, sql, new { newStatus, barcode, expectedStatus });

        return affected == 1;
    }
    // ========== 按BookID查询,返回 table_Book_Dto ==========
    public async Task<table_Book_Dto?> GetByIdAsync(int bookId, CancellationToken ct = default)
    {
        const string sql = @"SELECT BookID, Barcode, Status, ShelfID, ISBN FROM Book WHERE BookID = :bookId";

        using var connection = new Oracle.ManagedDataAccess.Client.OracleConnection(_connectionString);
        await connection.OpenAsync();

        return await Dapper.SqlMapper.QueryFirstOrDefaultAsync<table_Book_Dto>(
            connection, sql, new { bookId });
    }

    // ========== 按条码查询,返回 table_Book_Dto ==========
    public async Task<table_Book_Dto?> GetByBarcodeAsync(string barcode, CancellationToken ct = default)
    {
        const string sql = @"SELECT BookID, Barcode, Status, ShelfID, ISBN FROM Book WHERE Barcode = :barcode";

        using var connection = new Oracle.ManagedDataAccess.Client.OracleConnection(_connectionString);
        await connection.OpenAsync();

        return await Dapper.SqlMapper.QueryFirstOrDefaultAsync<table_Book_Dto>(
            connection, sql, new { barcode });
    }
}