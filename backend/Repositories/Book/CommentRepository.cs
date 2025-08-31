public class CommentRepository
{
    private readonly string _connectionString;

    public CommentRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<CommentDetailDto>> SearchCommentAsync(string ISBN)
    {
        var sql = @"
            SELECT *
            FROM COMMENT_TABLE
            WHERE ISBN = :ISBN";

        using var connection = new Oracle.ManagedDataAccess.Client.OracleConnection(_connectionString);
        await connection.OpenAsync();

        return await Dapper.SqlMapper.QueryAsync<CommentDetailDto>(connection, sql, new { ISBN = ISBN });
    }
    
    public async Task<int> AddCommentAsync(CommentDetailDto commentDto)
    {
        var sql = @"
            INSERT INTO Comment_Table (READERID, ISBN, RATING, REVIEWCONTENT, CREATETIME, STATUS)
            VALUES (:ReaderID, :ISBN, :Rating, :REVIEWCONTENT, :CREATETIME, :Status)";

        using var connection = new Oracle.ManagedDataAccess.Client.OracleConnection(_connectionString);
        await connection.OpenAsync();
        
        return await Dapper.SqlMapper.ExecuteAsync(connection, sql, commentDto);
    }
}