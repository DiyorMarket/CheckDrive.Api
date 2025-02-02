using CheckDrive.Application.DTOs.Reports;

namespace CheckDrive.Application.Interfaces.Reports;

public interface IReportGenerator
{
    public MemoryStream Generate(MonthlyReportSummary summary);
}
