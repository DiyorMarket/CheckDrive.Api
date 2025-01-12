using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.DTOs.Dashboard;

public sealed record CheckPointSummary(
    int Id,
    DateTime StartDate,
    string DriverName,
    string Car,
    CheckPointStage Stage,
    CheckPointStatus Status);
