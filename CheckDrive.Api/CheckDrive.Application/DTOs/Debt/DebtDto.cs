using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.DTOs.Debt;

public sealed record DebtDto(
    int CheckPointId,
    decimal FualAmount,
    decimal PaidAmount,
    DebtStatus Status);
