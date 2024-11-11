using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.QueryParameters;

public record CarQueryParameters(
    string? SearchText,
    CarStatus? Status);
