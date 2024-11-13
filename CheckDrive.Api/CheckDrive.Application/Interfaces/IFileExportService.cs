namespace CheckDrive.Application.Interfaces;
public interface IFileExportService
{
    Task<MemoryStream> ExportPdf();
    Task<MemoryStream> ExportExcel();
}
