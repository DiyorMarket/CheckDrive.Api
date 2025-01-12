using CheckDrive.Application.DTOs.Employee;
using CheckDrive.Application.QueryParameters;

namespace CheckDrive.Application.Interfaces;

public interface IEmployeeService
{
    Task<List<EmployeeDto>> GetAsync(EmployeeQueryParameters queryParameters);
    Task<EmployeeDto> GetByIdAsync(int id);
    Task<EmployeeDto> CreateAsync(CreateEmployeeDto account);
    Task<EmployeeDto> UpdateAsync(UpdateEmployeeDto account);
    Task DeleteAsync(int id);
}
