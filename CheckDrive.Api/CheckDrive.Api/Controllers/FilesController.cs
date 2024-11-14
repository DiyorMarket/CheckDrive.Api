using CheckDrive.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CheckDrive.Domain.Enums;

namespace CheckDrive.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FilesController : ControllerBase
{
    private readonly IFileExportService _fileExportService;

    public FilesController(IFileExportService fileExportService)
    {
        _fileExportService = fileExportService;
    }

    [HttpGet("export/pdf/employees")]
    public async Task<IActionResult> ExportEmployeesPdf(EmployeePosition position)
    {
        var stream = await _fileExportService.ExportPdf(position);

        var fileName = "Ishchilar ma'lumoti.pdf";
        var contentType = "application/pdf";

        return File(stream, contentType, fileName);
    }

    [HttpGet("export/pdf/cars")]
    public async Task<IActionResult> ExportCarsPdf()
    {
        var stream = await _fileExportService.ExportCarsPdf();
        
        var fileName = "Avtomobillar ma'lumoti.pdf";
        var contentType = "application/pdf";

        return File(stream, contentType, fileName);
    }

    [HttpGet("export/excel/employees")]
    public async Task<IActionResult> ExportExcel(EmployeePosition position)
    {
        var stream = await _fileExportService.ExportExcel(position);

        var fileName = "Ishchilar ma'lumoti.xlsx";
        var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(stream, contentType, fileName);
    }

    [HttpGet("export/excel/cars")]
    public async Task<IActionResult> ExportCarsExcel()
    {
        var stream = await _fileExportService.ExportCarsExcel();

        var fileName = "Avtomobillar ma'lumoti.xlsx";
        var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(stream, contentType, fileName);
    }
}
