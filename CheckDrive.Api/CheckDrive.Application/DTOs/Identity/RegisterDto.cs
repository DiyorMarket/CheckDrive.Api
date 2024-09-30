using CheckDrive.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CheckDrive.Application.DTOs.Identity;
public class RegisterDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Passport { get; set; }
    public required string Address { get; set; }
    public required DateTime Birthdate { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required EmployeePosition Position { get; set; }
}
