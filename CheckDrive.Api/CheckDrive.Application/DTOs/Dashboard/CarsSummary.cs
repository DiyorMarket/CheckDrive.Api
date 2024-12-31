namespace CheckDrive.Application.DTOs.Dashboard;

public sealed record CarsSummary(
    int FreeCarsCount,
    int OutOfServiceCarsCount,
    int BusyCarsCount);