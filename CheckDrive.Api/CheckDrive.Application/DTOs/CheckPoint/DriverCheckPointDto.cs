using CheckDrive.Application.DTOs.Car;
using CheckDrive.Application.DTOs.Review;
using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.DTOs.CheckPoint;

public sealed record DriverCheckPointDto(
    int Id,
    DateTime StartDate,
    CheckPointStage Stage,
    CheckPointStatus Status,
    string DriverName,
    CarDto? Car,
    List<DriverReviewDto> Reviews);
