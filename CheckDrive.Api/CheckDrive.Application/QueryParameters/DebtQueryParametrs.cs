using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.QueryParameters;

public sealed record DebtQueryParametrs(
    string? SearchText, DebtStatus? Status);
