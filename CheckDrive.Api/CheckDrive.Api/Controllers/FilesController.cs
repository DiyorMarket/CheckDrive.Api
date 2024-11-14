using CheckDrive.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CheckDrive.Domain.Enums;
using CheckDrive.Domain.QueryParameters;

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

    [HttpPost("import/employees")]
    public async Task<IActionResult> ImportEmployees(IFormFile file)
    {
        await _fileExportService.ImportEmployees(file);

        return Ok();
    }

    [HttpGet("export/employees")]
    public async Task<IActionResult> ExportEmployeesPdf([FromQuery] EmployeePosition position,
        [FromQuery] FileQueryParameters queryParameters)
    {
        var stream = await _fileExportService.Export(position, queryParameters);

        var contentType = queryParameters.FileType == FileType.Pdf
             ? "application/pdf"
             : "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        var fileName = queryParameters.FileType == FileType.Pdf
            ? "Ishchilar ma'lumoti.pdf"
            : "Ishchilar ma'lumoti.xlsx";

        return File(stream, contentType, fileName);
    }

    [HttpGet("export/cars")]
    public async Task<IActionResult> ExportCars([FromQuery] FileQueryParameters queryParameters)
    {
        var stream = await _fileExportService.ExportCars(queryParameters);
        
        var contentType = queryParameters.FileType == FileType.Pdf 
            ? "application/pdf" 
            : "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        
        var fileName = queryParameters.FileType == FileType.Pdf 
            ? "Avtomobillar ma'lumoti.pdf"
            : "Avtomobillar ma'lumoti.xlsx";

        return File(stream, contentType, fileName);
    }
}
