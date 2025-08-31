using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.DTOs.Admin;
using backend.Repositories.Admin;

namespace backend.Services.Admin
{
    public class ReportService
    {
        private readonly ReportRepository _repository;

        public ReportService(ReportRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<ReportDetailDto>> GetPendingReportsAsync()
        {
            return _repository.GetPendingReportsAsync();
        }

        public async Task<bool> HandleReportAsync(int reportId, int commentId, string action, int adminId)
        {
            return action.ToLower() switch
            {
                // 同意举报 -> 删除评论，举报状态变为“处理完成”
                "approve" => await _repository.HandleReportAsync(reportId, commentId, "处理完成", "已删除", adminId),
                
                // 驳回举报 -> 评论不变，举报状态变为“驳回”
                "reject" => await _repository.HandleReportAsync(reportId, commentId, "驳回", null, adminId),
                
                _ => throw new ArgumentException("Invalid action specified.", nameof(action))
            };
        }
    }
}