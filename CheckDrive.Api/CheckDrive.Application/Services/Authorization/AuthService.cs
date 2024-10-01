using AutoMapper;
using CheckDrive.Application.Constants;
using CheckDrive.Application.DTOs.Account;
using CheckDrive.Application.DTOs.Identity;
using CheckDrive.Application.Interfaces;
using CheckDrive.Application.Interfaces.Authorization;
using CheckDrive.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace CheckDrive.Application.Services.Authorization;

public class AuthService : IAuthService
{
    private readonly JwtHandler _jwtHandler;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IAccountService _accountService;
    private readonly IMapper _mapper;

    public AuthService(
        IMapper mapper,
        JwtHandler jwtHandler,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IAccountService accountService
    )
    {
        _jwtHandler = jwtHandler ?? throw new ArgumentNullException(nameof(jwtHandler));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        _accountService = accountService ?? throw new ArgumentNullException(nameof(roleManager));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<string> LoginAsync(LoginDto loginDto)
    {
        var user = await _userManager.FindByNameAsync(loginDto.UserName)
            ?? throw new InvalidLoginAttemptException("Invalid email or password");

        if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            throw new InvalidLoginAttemptException("Invalid email or password");
        }
        var roles = await _userManager.GetRolesAsync(user);

        string token = _jwtHandler.GenerateToken(user,roles);

        return token;
    }

    public async Task RegisterAsync(RegisterDto registerDto)
    {
        var account = CreateAccountDto(registerDto);

        await _accountService.CreateAsync(account);
        await AssignRole(registerDto);
    }

    private CreateAccountDto CreateAccountDto(RegisterDto registerDto) 
        => new (registerDto.Username,
            registerDto.Password,
            registerDto.PasswordConfirm,
            registerDto.PhoneNumber,
            registerDto.Email,
            registerDto.FirstName,
            registerDto.LastName,
            registerDto.Address,
            registerDto.Passport,
            registerDto.Birthdate,
            Domain.Enums.EmployeePosition.Manager);

    private async Task AssignRole(RegisterDto registerDto)
    {
        var user = await _userManager.FindByNameAsync(registerDto.Username)
            ?? throw new RegistrationFailedException();
       
        if (!await _roleManager.RoleExistsAsync(Roles.Manager))
        {
            throw new InvalidOperationException("The role does not exist.");
        }
        var roleResult = await _userManager.AddToRoleAsync(user, Roles.Manager);
       
        if (!roleResult.Succeeded)
        {
            await _userManager.DeleteAsync(user);
            throw new RegistrationFailedException(string.Join(", ", roleResult.Errors.Select(e => e.Description)));
        }
    }
}
