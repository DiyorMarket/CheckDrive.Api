﻿using System;

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
        public DateTime Bithdate { get; set; }
        public string RoleName { get; set; }
        public int RoleId { get; set; }
    }
}
