using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.QueryParameters;

public sealed record DriverQueryParameters(string? SearchText, DriverStatus? Status = DriverStatus.Available);
