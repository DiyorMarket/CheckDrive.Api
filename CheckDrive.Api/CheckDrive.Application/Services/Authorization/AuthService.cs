using AutoMapper;
using CheckDrive.Application.DTOs.Identity;
using CheckDrive.Application.Interfaces.Authorization;
using CheckDrive.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace CheckDrive.Application.Services.Authorization;

public class AuthService(
    IMapper mapper,
    JwtHandler jwtHandler,
    UserManager<IdentityUser> userManager,
    RoleManager<IdentityRole> roleManager,
    IEmployeeFactory employeeFactory
    ) : IAuthService
{
    private readonly JwtHandler _jwtHandler = jwtHandler 
        ?? throw new ArgumentNullException(nameof(jwtHandler));

    private readonly UserManager<IdentityUser> _userManager = userManager 
        ?? throw new ArgumentNullException(nameof(userManager));

    private readonly RoleManager<IdentityRole> _roleManager = roleManager 
        ?? throw new ArgumentNullException(nameof(roleManager));

    private readonly IMapper _mapper = mapper 
        ?? throw new ArgumentNullException(nameof(mapper));

    private readonly IEmployeeFactory _employeeFactory = employeeFactory 
        ?? throw new ArgumentNullException(nameof(employeeFactory));

    public async Task<string> LoginAsync(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Login)
            ?? throw new InvalidLoginAttemptException();

        ValidateUserForLogin(user, loginDto.Password);

        return await _jwtHandler.GenerateTokenAsync(user);
    }

    public async Task RegisterAsync(RegisterDto registerDto)
    {
        //Create a new Identity User and assign his password
        var user = await CreateUser(registerDto);

        //Assign role a new Identity User 
        await AssignRole(user, registerDto.Position.ToString());

        //Create Employee with required properties
        await _employeeFactory.CreateEmployee(registerDto, user);
    }

    public async Task RegisterAdministratorAsync(RegisterDto registerDto)
    {
        //Create new Identity User and assign password
        var user = await CreateUser(registerDto);
        //Assign role to user
        await AssignRole(user, Roles.Administrator);
    }
    private async void ValidateUserForLogin(IdentityUser? user, string password)
    {
        if (user == null || !user.EmailConfirmed || !await _userManager.CheckPasswordAsync(user, password))
        {
            throw new InvalidLoginAttemptException();
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
