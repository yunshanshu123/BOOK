using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.DTOs.Admin;
using backend.Services.Admin;

namespace backend.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/reports")]
    public class ReportController : ControllerBase
    {
        private readonly ReportService _service;

        public ReportController(ReportService service)
        {
            _service = service;
        }

        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<ReportDetailDto>>> GetPendingReports()
        {
            var reports = await _service.GetPendingReportsAsync();
            return Ok(reports);
        }

        [HttpPut("{reportId}/handle")]
        public async Task<IActionResult> HandleReport(int reportId, [FromBody] HandleReportDto dto)
        {
            try
            {
                // In a real app, the adminId should be retrieved from the user's authentication context.
                var adminId = 1; // Placeholder for logged-in admin's ID

                // Now we get CommentId directly and securely from the request body.
                var success = await _service.HandleReportAsync(reportId, dto.CommentId, dto.Action, adminId);
                
                if (success)
                {
                    return NoContent(); // HTTP 204: Success, no content to return.
                }
                return NotFound("Report not found or could not be updated.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}