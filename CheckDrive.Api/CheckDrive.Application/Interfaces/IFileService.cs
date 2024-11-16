using CheckDrive.Domain.Enums;
using CheckDrive.Domain.QueryParameters;
using Microsoft.AspNetCore.Http;

namespace CheckDrive.Application.Interfaces;

public interface IFileService
{
    Task<MemoryStream> ExportEmployees(EmployeePosition position, FileQueryParameters queryParameters);
    Task<MemoryStream> ExportCars(FileQueryParameters queryParameters);
    Task ImportEmployees(IFormFile file);
}
