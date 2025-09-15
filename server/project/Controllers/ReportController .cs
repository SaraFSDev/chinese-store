using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using project.Services.IServices;
using Microsoft.AspNetCore.Authorization;

namespace project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [Authorize(Roles = "manager")]
        [HttpGet("excel/winners")]
        [Produces("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
        public async Task<IActionResult> ExportWinnersToExcel()
        {
            var file = await _reportService.GenerateWinnersReport();
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "WinnersReport.xlsx");
        }

        [HttpGet("excel/revenue")]
        [Produces("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> ExportRevenueToExcel()
        {
            var file = await _reportService.GenerateRevenueReportAsync();
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "RevenueReport.xlsx");
        }
    }
}



