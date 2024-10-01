using CheckDrive.Application.Interfaces.Authorization;
using CheckDrive.Application.Interfaces.Review;
using CheckDrive.Application.Services.Authorization;
﻿using CheckDrive.Application.Interfaces;
using CheckDrive.Application.Services;
using CheckDrive.Application.Services.Review;
using CheckDrive.Domain.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CheckDrive.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection RegisterApplication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        AddServices(services);
        AddConfigurations(services,configuration);
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        return services;
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddScoped<JwtHandler>();    
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IDoctorReviewService, DoctorReviewService>();
        services.AddScoped<IMechanicHandoverService, MechanicHandoverService>();
        services.AddScoped<IOperatorReviewService, OperatorReviewService>();
        services.AddScoped<IMechanicAcceptanceService, MechanicAcceptanceService>();
        services.AddScoped<IDispatcherReviewService, DispatcherReviewService>();
        services.AddScoped<ICheckPointService, CheckPointService>();
        services.AddScoped<IAccountService, AccountService>();
    }
    private static void AddConfigurations(IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<JwtOptions>()
            .Bind(configuration.GetSection(JwtOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }
}
