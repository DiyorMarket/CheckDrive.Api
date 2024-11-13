using CheckDrive.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using Syncfusion.Pdf;
using System.Data;
using Syncfusion.Drawing;
using CheckDrive.Application.DTOs.CheckPoint;

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

    [HttpGet("drivers/pdf")]
    public async Task<IActionResult> GetDriversPdfAsync()
    {
        var stream = await _fileExportService.ExportPdf();

        return File(stream, "application/pdf", "DriverInformation.pdf");
    }

    [HttpGet("export-excel")]
    public async Task<IActionResult> ExportExcel()
    {
        var excelStream = await _fileExportService.ExportExcel();

        string fileName = "DriverDetails.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(excelStream, contentType, fileName);
    }
}
