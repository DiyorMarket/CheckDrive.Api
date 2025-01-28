using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.DTOs.Debt;

public record class DebtDto(
    int Id,
    string FirsName,
    string LastName,
    decimal FualAmount,
    decimal PaidAmount,
    DebtStatus Status);
