using CheckDrive.Application.Interfaces.Reports;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers;

[Route("api/reports")]
[ApiController]
public class ReportsController(IReportService reportService) : ControllerBase
{
    [HttpGet("monthly")]
    public async Task<IActionResult> GetMonthlyReport()
    {
        var reportStream = await reportService.GenerateReportForCurrentMonthAsync();

        return File(reportStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Monthly_Car_Usage_Report.xlsx");
    }
}
