using CheckDrive.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace CheckDrive.Domain.Common;

public class Employee : EntityBase
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Passport { get; set; }
    public required string Address { get; set; }
    public required DateTime Birthdate { get; set; }
    public required EmployeePosition Position { get; set; }

    public required string AccountId { get; set; }
    public required virtual IdentityUser Account { get; set; }
}
