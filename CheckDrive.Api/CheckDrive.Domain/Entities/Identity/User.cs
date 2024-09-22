using CheckDrive.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace CheckDrive.Domain.Entities.Identity;

public class User : IdentityUser<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Passport { get; set; }
    public string Address { get; set; }
    public DateTime Birthdate { get; set; }
    public EmployeePosition Position { get; set; }
}
