using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.QueryParameters;

public sealed record EmployeeQueryParameters(string? SearchText, EmployeePosition? Position);
