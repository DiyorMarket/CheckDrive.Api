using CheckDrive.Application.DTOs.Debt;
using CheckDrive.Application.DTOs.Review;
using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.DTOs.CheckPoint;

public sealed record CheckPointDto(
    int Id,
    DateTime StartDate,
    CheckPointStatus Status,
    CheckPointStage Stage,
    List<ReviewDtoBase> Reviews,
    DebtDto? Debt);
