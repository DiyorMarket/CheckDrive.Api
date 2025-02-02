using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.DTOs.CheckPoint;

public sealed record CheckPointDto(
    int Id,
    DateTime StartDate,
    CheckPointStage Stage,
    CheckPointStatus Status,
    string Driver,
    string CarModel,
    decimal CurrentFuelAmount,
    string Mechanic,
    decimal InitialMillage,
    decimal FinalMileage,
    string Operator,
    decimal InitialOilAmount,
    decimal OilRefillAmount,
    string Oil,
    string Dispatcher,
    decimal FuelConsumptionAdjustment,
    decimal DebtAmount
);
