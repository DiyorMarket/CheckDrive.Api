using CheckDrive.Application.DTOs.Identity;
using CheckDrive.Application.Interfaces.Authorization;
using CheckDrive.Domain.Common;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Enums;
using CheckDrive.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CheckDrive.Application.Services.Authorization;

internal sealed class EmployeeFactory : IEmployeeFactory
{
    private readonly ICheckDriveDbContext _dbContext;

    public EmployeeFactory(ICheckDriveDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task CreateEmployee(RegisterDto registerDto, IdentityUser user)
    {
        ArgumentNullException.ThrowIfNull(registerDto);
        ArgumentNullException.ThrowIfNull(user);

        switch (registerDto.Position)
        {
            case EmployeePosition.Dispatcher:
                await CreateDispatcherAsync(registerDto, user);
                break;
            case EmployeePosition.Doctor:
                await CreateDoctorAsync(registerDto, user);
                break;
            case EmployeePosition.Driver:
                await CreateDriverAsync(registerDto, user);
                break;
            case EmployeePosition.Mechanic:
                await CreateMechanicAsync(registerDto, user);
                break;
            case EmployeePosition.Manager:
                await CreateManagerAsync(registerDto, user);
                break;
            case EmployeePosition.Operator:
                await CreateOperatorAsync(registerDto, user);
                break;
            default:
                throw new InvalidOperationException("Invalid position");
        }

        await _dbContext.SaveChangesAsync();
    }

    private async Task CreateDispatcherAsync(RegisterDto registerDto, IdentityUser user)
    {
        var dispatcher = new Dispatcher
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Passport = registerDto.Passport,
            Address = registerDto.Address,
            Birthdate = registerDto.Birthdate,
            Position = EmployeePosition.Dispatcher,
            AccountId = user.Id,
            Account = user
        };
        await _dbContext.Dispatchers.AddAsync(dispatcher);
    }

    private async Task CreateDoctorAsync(RegisterDto registerDto, IdentityUser user)
    {
        var doctor = new Doctor
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Passport = registerDto.Passport,
            Address = registerDto.Address,
            Birthdate = registerDto.Birthdate,
            Position = EmployeePosition.Doctor,
            AccountId = user.Id,
            Account = user
        };
        await _dbContext.Doctors.AddAsync(doctor);
    }

    private async Task CreateDriverAsync(RegisterDto registerDto, IdentityUser user)
    {
        var driver = new Driver
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Passport = registerDto.Passport,
            Address = registerDto.Address,
            Birthdate = registerDto.Birthdate,
            Position = EmployeePosition.Driver,
            AccountId = user.Id,
            Account = user
        };
        await _dbContext.Drivers.AddAsync(driver);
    }

    private async Task CreateMechanicAsync(RegisterDto registerDto, IdentityUser user)
    {
        var mechanic = new Mechanic
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Passport = registerDto.Passport,
            Address = registerDto.Address,
            Birthdate = registerDto.Birthdate,
            Position = EmployeePosition.Mechanic,
            AccountId = user.Id,
            Account = user
        };
        await _dbContext.Mechanics.AddAsync(mechanic);
    }

    private async Task CreateManagerAsync(RegisterDto registerDto, IdentityUser user)
    {
        var manager = new Manager
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Passport = registerDto.Passport,
            Address = registerDto.Address,
            Birthdate = registerDto.Birthdate,
            Position = EmployeePosition.Manager,
            AccountId = user.Id,
            Account = user
        };
        //  await _dbContext.Managers.AddAsync(manager);
    }

    private async Task CreateOperatorAsync(RegisterDto registerDto, IdentityUser user)
    {
        var @operator = new Operator
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Passport = registerDto.Passport,
            Address = registerDto.Address,
            Birthdate = registerDto.Birthdate,
            Position = EmployeePosition.Operator,
            AccountId = user.Id,
            Account = user
        };
        await _dbContext.Operators.AddAsync(@operator);
    }
}
