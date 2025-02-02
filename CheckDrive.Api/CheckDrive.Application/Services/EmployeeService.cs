using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CheckDrive.Application.Constants;
using CheckDrive.Application.Interfaces;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Enums;
using CheckDrive.Domain.Exceptions;
using CheckDrive.Domain.Interfaces;
using CheckDrive.Application.QueryParameters;
using AutoMapper.QueryableExtensions;
using CheckDrive.Application.DTOs.Employee;

namespace CheckDrive.Application.Services;

internal sealed class EmployeeService(
    ICheckDriveDbContext context,
    IMapper mapper,
    UserManager<IdentityUser> userManager) : IEmployeeService
{
    public async Task<List<EmployeeDto>> GetAsync(EmployeeQueryParameters queryParameters)
    {
        var query = GetQuery(queryParameters);

        var employees = await query
            .AsNoTracking()
            .OrderByDescending(x => x.Id)
            .ProjectTo<EmployeeDto>(mapper.ConfigurationProvider)
            .ToListAsync();

        return employees;
    }

    public async Task<EmployeeDto> GetByIdAsync(int id)
    {
        var account = await context.Employees
            .AsNoTracking()
            .Include(x => x.Account)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (account is null)
        {
            throw new EntityNotFoundException($"Account with id: {id} is not found.");
        }

        var dto = mapper.Map<EmployeeDto>(account);

        return dto;
    }

    public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var account = await CreateAccountAsync(request);
        var employee = await CreateEmployeeAsync(request, account);

        return mapper.Map<EmployeeDto>(employee);
    }

    // Update Employee & Accunt
    public async Task<EmployeeDto> UpdateAsync(UpdateEmployeeDto account)
    {
        ArgumentNullException.ThrowIfNull(account);

        var employee = mapper.Map<Employee>(account);

        context.Employees.Update(employee);
        await context.SaveChangesAsync();

        return mapper.Map<EmployeeDto>(employee);
    }

    public async Task DeleteAsync(int id)
    {
        var employee = await context.Employees
            .Include(x => x.Account)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (employee is null)
        {
            throw new EntityNotFoundException($"User account with id: {id} is not found.");
        }

        context.Users.Remove(employee.Account);
        await context.SaveChangesAsync();
    }

    private async Task<IdentityUser> CreateAccountAsync(CreateEmployeeDto request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var user = mapper.Map<IdentityUser>(request);
        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException("Could not create user account.");
        }

        await AssignToRoleAsync(user, request.Position);

        return user;
    }

    private async Task<Employee> CreateEmployeeAsync(CreateEmployeeDto request, IdentityUser account)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(account);

        var employee = mapper.Map<Employee>(request);
        employee.Account = account;

        context.Employees.Add(employee);
        await context.SaveChangesAsync();

        return employee;
    }

    private async Task AssignToRoleAsync(IdentityUser user, EmployeePosition position)
    {
        var role = position switch
        {
            EmployeePosition.Driver => Roles.Driver,
            EmployeePosition.Doctor => Roles.Doctor,
            EmployeePosition.Mechanic => Roles.Mechanic,
            EmployeePosition.Operator => Roles.Operator,
            EmployeePosition.Dispatcher => Roles.Dispatcher,
            EmployeePosition.Manager => Roles.Manager,
            _ => throw new InvalidOperationException("Invalid position for role assignment.")
        };

        var roleResult = await userManager.AddToRoleAsync(user, role.ToString());

        if (!roleResult.Succeeded)
        {
            throw new InvalidOperationException("Could not create user account.");
        }
    }

    private IQueryable<Employee> GetQuery(EmployeeQueryParameters queryParameters)
    {
        var query = context.Employees
            .Include(x => x.Account)
            .AsQueryable();

        if (!string.IsNullOrEmpty(queryParameters.SearchText))
        {
            query = query.Where(x => x.FirstName.Contains(queryParameters.SearchText)
                || x.LastName.Contains(queryParameters.SearchText)
                || x.Patronymic.Contains(queryParameters.SearchText)
                || x.Passport != null && x.Passport.Contains(queryParameters.SearchText)
                || x.Address != null && x.Address.Contains(queryParameters.SearchText)
                || x.Account.UserName != null && x.Account.UserName.Contains(queryParameters.SearchText));
        }

        if (queryParameters.Position.HasValue)
        {
            query = query.Where(x => x.Position == queryParameters.Position.Value);
        }

        query = query.OrderBy(x => x.FirstName).ThenBy(x => x.LastName);

        return query;
    }
}
