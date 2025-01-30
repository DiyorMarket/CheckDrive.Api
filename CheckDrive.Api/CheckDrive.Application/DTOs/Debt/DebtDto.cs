﻿using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.DTOs.Debt;

public record DebtDto(
    int Id,
    string FirstName,
    string LastName,
    decimal FualAmount,
    decimal PaidAmount,
    int CheckPointId,
    DebtStatus Status);
