using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.DTOs.Debt;

public record DebtDto(
    int Id,
    string DriverFullName,
    decimal FuelAmount,
    decimal PaidAmount,
    int CheckPointId,
    DebtStatus Status);
