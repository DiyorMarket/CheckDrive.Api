﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckDrive.Infrastructure.Models
{
    public class UserInfo
    {
        public string Email { get; init; }
        public string Name { get; init; }
        public string? Device { get; init; }
        public string? OperatingSystem { get; init; }

        public UserInfo(string email, string name)
        {
            Email = email;
            Name = name;
        }

        public UserInfo(string email, string name, string? device, string? operatingSystem)
            : this(email, name)
        {
            Device = device;
            OperatingSystem = operatingSystem;
        }
    }
}