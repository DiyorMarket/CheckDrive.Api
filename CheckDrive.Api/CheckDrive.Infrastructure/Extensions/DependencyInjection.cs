using CheckDrive.Domain.Interfaces;
using CheckDrive.Application.Interfaces;
using CheckDrive.Infrastructure.Configurations;
using CheckDrive.Infrastructure.Email;
using CheckDrive.Infrastructure.Persistence;
using CheckDrive.Infrastructure.Sms;
using FluentEmail.MailKitSmtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CheckDrive.Application.Configurations;

namespace CheckDrive.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection RegisterInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddPersistence(services, configuration);
        AddConfigurations(services, configuration);
        AddEmail(services, configuration);
        AddServices(services);
        AddIdentity(services);

        return services;
    }

    private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ICheckDriveDbContext, CheckDriveDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    }

    private static void AddConfigurations(IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<JwtOptions>()
            .Bind(configuration.GetSection(JwtOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<EmailConfigurations>()
            .Bind(configuration.GetSection(EmailConfigurations.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<SmsConfigurations>()
            .Bind(configuration.GetSection(SmsConfigurations.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }

    private static void AddEmail(IServiceCollection services, IConfiguration configuration)
    {
        var emailSettings = configuration
            .GetSection(EmailConfigurations.SectionName)
            .Get<EmailConfigurations>();

        if (emailSettings is null)
        {
            throw new InvalidOperationException("Configuration values for email did not load correctly.");
        }

        var smptOptions = new SmtpClientOptions
        {
            Server = emailSettings.Server,
            Port = emailSettings.Port,
            User = emailSettings.From,
            Password = emailSettings.Password,
            UseSsl = true,
            RequiresAuthentication = true
        };

        services.AddFluentEmail(emailSettings.From, emailSettings.UserName)
              .AddMailKitSender(smptOptions)
              .AddRazorRenderer();
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ISmsService, SmsService>();
    }

    private static void AddIdentity(IServiceCollection services)
    {
        services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            options.Password.RequiredLength = 7;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireDigit = false;
        })
            .AddEntityFrameworkStores<CheckDriveDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<DataProtectionTokenProviderOptions>(options =>
        {
            options.TokenLifespan = TimeSpan.FromHours(12);
        });
    }
}
