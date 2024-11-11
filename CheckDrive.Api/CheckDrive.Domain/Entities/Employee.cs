using CheckDrive.Domain.Common;
using CheckDrive.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace CheckDrive.Domain.Entities;

public class Employee : EntityBase
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Patronymic { get; set; }
    public string? Passport { get; set; }
    public string? Address { get; set; }
    public DateTime? Birthdate { get; set; }
    public required EmployeePosition Position { get; set; }
    public string? PositionDescription { get; set; }

    public required string AccountId { get; set; }
    public required virtual IdentityUser Account { get; set; }
}
