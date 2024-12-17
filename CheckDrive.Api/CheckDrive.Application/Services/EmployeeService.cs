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

internal sealed class EmployeeService : IEmployeeService
{
    private readonly ICheckDriveDbContext _context;
    private readonly IMapper _mapper;
    private readonly UserManager<IdentityUser> _userManager;

    public EmployeeService(
        ICheckDriveDbContext context,
        IMapper mapper,
        UserManager<IdentityUser> userManager)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task<List<EmployeeDto>> GetAsync(EmployeeQueryParameters queryParameters)
    {
        var query = GetQuery(queryParameters);

        var employees = await query
            .AsNoTracking()
            .ProjectTo<EmployeeDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return employees;
    }

    public async Task<EmployeeDto> GetByIdAsync(string id)
    {
        ArgumentNullException.ThrowIfNull(id);

        var account = await _context.Employees
            .AsNoTracking()
            .Include(x => x.Account)
            .FirstOrDefaultAsync(x => x.AccountId == id);

        if (account is null)
        {
            throw new EntityNotFoundException($"Account with id: {id} is not found.");
        }

        var dto = _mapper.Map<EmployeeDto>(account);

        return dto;
    }

    public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto account)
    {
        ArgumentNullException.ThrowIfNull(account);

        var user = _mapper.Map<IdentityUser>(account);
        var result = await _userManager.CreateAsync(user, account.PasswordConfirm);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException("Could not create user account.");
        }

        await AssignToRoleAsync(user, account.Position);

        var employee = _mapper.Map<Employee>(account);
        employee.AccountId = user.Id;

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        employee.Account = user;
        var dto = _mapper.Map<EmployeeDto>(employee);

        return dto;
    }

    // Update Employee & Accunt
    public Task<EmployeeDto> UpdateAsync(UpdateEmployeeDto account)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(string id)
    {
        var account = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

        if (account is null)
        {
            throw new EntityNotFoundException($"User account with id: {id} is not found.");
        }

        _context.Users.Remove(account);
        await _context.SaveChangesAsync();
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

        var roleResult = await _userManager.AddToRoleAsync(user, role.ToString());

        if (!roleResult.Succeeded)
        {
            throw new InvalidOperationException("Could not create user account.");
        }
    }

    private IQueryable<Employee> GetQuery(EmployeeQueryParameters queryParameters)
    {
        var query = _context.Employees
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
