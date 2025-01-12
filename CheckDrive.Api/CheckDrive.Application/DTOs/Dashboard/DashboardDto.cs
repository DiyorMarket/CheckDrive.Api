namespace CheckDrive.Application.DTOs.Dashboard;

public sealed record DashboardDto(
    CarsSummary CarsSummary,
    List<EmployeeSummary> EmployeeSummaries,
    List<OilConsumptionSummary> OilConsumptionSummaries,
    List<CheckPointSummary> CheckPointSummaries);
