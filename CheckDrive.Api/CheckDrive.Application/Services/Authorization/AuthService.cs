using AutoMapper;
using CheckDrive.Application.Constants;
using CheckDrive.Application.DTOs.Identity;
using CheckDrive.Application.Interfaces.Authorization;
using CheckDrive.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace CheckDrive.Application.Services.Authorization;

public class AuthService : IAuthService
{
    private readonly JwtHandler _jwtHandler;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMapper _mapper;
    private readonly IEmployeeRegistrationService _employeeFactory;

    public AuthService(
        IMapper mapper,
        JwtHandler jwtHandler,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IEmployeeRegistrationService employeeFactory
    )
    {
        _jwtHandler = jwtHandler ?? throw new ArgumentNullException(nameof(jwtHandler));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _employeeFactory = employeeFactory ?? throw new ArgumentNullException(nameof(employeeFactory));
    }

    public async Task<string> LoginAsync(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email)
            ?? throw new InvalidLoginAttemptException("Invalid email or password");

        ValidateUserForLogin(user, loginDto.Password);

        return await _jwtHandler.GenerateTokenAsync(user);
    }

    public async Task RegisterEmployeeAsync(RegisterDto registerDto)
    {
        var user = await CreateUser(registerDto);

        await AssignRole(user, registerDto.Position.ToString());

        await _employeeFactory.CreateEmployee(registerDto, user);
    }

    public async Task RegisterAdministratorAsync(RegisterDto registerDto)
    {
        var user = await CreateUser(registerDto);
        await AssignRole(user, Roles.Administrator);
    }
    private async void ValidateUserForLogin(IdentityUser? user, string password)
    {
        if (user == null || !user.EmailConfirmed || !await _userManager.CheckPasswordAsync(user, password))
        {
            throw new InvalidLoginAttemptException("Invalid email or password");
        }
    }

    private async Task<IdentityUser> CreateUser(RegisterDto registerDto)
    {
        var user = new IdentityUser
        {
            UserName = registerDto.Email,
            Email = registerDto.Email,
            EmailConfirmed = true
        };

        var createUserResult = await _userManager.CreateAsync(user, registerDto.Password);
        if (!createUserResult.Succeeded)
        {
            throw new RegistrationFailedException((string.Join(", ", createUserResult.Errors.Select(e => e.Description))));
        }

        return user;
    }

    private async Task AssignRole(IdentityUser user, string position)
    {
        if (!await _roleManager.RoleExistsAsync(position))
        {
            throw new InvalidOperationException("The role does not exist.");
        }

        var roleResult = await _userManager.AddToRoleAsync(user, position.ToString());
        if (!roleResult.Succeeded)
        {
            await _userManager.DeleteAsync(user);
            throw new RegistrationFailedException(string.Join(", ", roleResult.Errors.Select(e => e.Description)));
        }
    }
}
