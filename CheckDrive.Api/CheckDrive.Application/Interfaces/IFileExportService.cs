using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.Interfaces;
public interface IFileExportService
{
    Task<MemoryStream> ExportPdf(EmployeePosition position);
    Task<MemoryStream> ExportExcel(EmployeePosition position);
    Task<MemoryStream> ExportCarsExcel();
    Task<MemoryStream> ExportCarsPdf();
}
