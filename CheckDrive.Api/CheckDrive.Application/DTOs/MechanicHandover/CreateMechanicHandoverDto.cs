using CheckDrive.Application.DTOs.Review;
using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.DTOs.MechanicHandover;

public record CreateMechanicHandoverDto(
    int CarId,
    Guid DriverId,
    int InitialMileage,
    string? Notes,
    DateTime Date,
    ReviewStatus Status)
    : ReviewDtoBase(
        Notes: Notes,
        Date: Date,
        Status: Status);
