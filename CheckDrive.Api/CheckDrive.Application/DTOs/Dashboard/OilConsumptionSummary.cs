namespace CheckDrive.Application.DTOs.Dashboard;

public sealed record OilConsumptionSummary(
    string Month,
    decimal Ai80,
    decimal Ai91,
    decimal Ai92,
    decimal Ai95,
    decimal Diesel);
