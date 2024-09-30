using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckDrive.Application.DTOs.Identity;
public class Roles
{
    public const string Administrator = nameof(Administrator);

    public const string Driver = nameof(Driver);
    public const string Doctor = nameof(Doctor);
    public const string Manager = nameof(Manager);
    public const string Operator = nameof(Operator);
    public const string Mechanic = nameof(Mechanic);
    public const string Dispatcher = nameof(Dispatcher);
}
