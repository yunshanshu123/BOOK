using Dapper;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using backend.DTOs.Admin;

namespace backend.Repositories.Admin
{
    public class ReportRepository
    {
        private readonly string _connectionString;

        public ReportRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<ReportDetailDto>> GetPendingReportsAsync()
        {
            var sql = "SELECT * FROM V_ReportDetails WHERE ReportStatus = '待处理' ORDER BY ReportTime ASC";
            using var connection = new OracleConnection(_connectionString);
            return await connection.QueryAsync<ReportDetailDto>(sql);
        }

        public async Task<bool> HandleReportAsync(int reportId, int commentId, string newReportStatus, string? newCommentStatus, int librarianId)
        {
            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();

            try
            {
                // Step 1: Update the Report status
                var reportUpdateSql = "UPDATE Report SET Status = :NewReportStatus, LibrarianID = :LibrarianID WHERE ReportID = :ReportID";
                var reportRowsAffected = await connection.ExecuteAsync(reportUpdateSql, 
                    new { NewReportStatus = newReportStatus, LibrarianID = librarianId, ReportID = reportId }, 
                    transaction);

                // Step 2: If a new comment status is provided (i.e., we are deleting the comment), update it
                if (!string.IsNullOrEmpty(newCommentStatus))
                {
                    var commentUpdateSql = "UPDATE Comment_Table SET Status = :NewCommentStatus WHERE CommentID = :CommentID";
                    await connection.ExecuteAsync(commentUpdateSql, 
                        new { NewCommentStatus = newCommentStatus, CommentID = commentId }, 
                        transaction);
                }

                transaction.Commit();
                return reportRowsAffected > 0;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}