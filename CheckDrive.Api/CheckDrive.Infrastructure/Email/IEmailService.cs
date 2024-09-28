using CheckDrive.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckDrive.Infrastructure.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailMetadata metadata);
    }
}
