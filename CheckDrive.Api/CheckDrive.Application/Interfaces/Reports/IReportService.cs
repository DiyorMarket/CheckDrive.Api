namespace CheckDrive.Application.Interfaces.Reports;

public interface IReportService
{
    public Task<MemoryStream> GenerateReportForCurrentMonthAsync();
    public Task<FileStream> GenerateReportAsync(DateTime start, DateTime end);
}
