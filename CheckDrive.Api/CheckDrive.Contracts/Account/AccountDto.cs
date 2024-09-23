using System;

namespace CheckDrive.ApiContracts.Account
{
    public class AccountDto
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Position { get; set; }
        public string Passport { get; set; }
        public DateTime Bithdate { get; set; }
        public string RoleName { get; set; }
        public int RoleId { get; set; }
    }
}
