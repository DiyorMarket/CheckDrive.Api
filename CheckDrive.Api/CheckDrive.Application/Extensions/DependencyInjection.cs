using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using CheckDrive.Application.Interfaces.Review;
using CheckDrive.Application.Interfaces;
using CheckDrive.Application.Services;
using CheckDrive.Application.Services.Review;
using CheckDrive.Application.Interfaces.Auth;
using CheckDrive.Application.Services.Auth;
using CheckDrive.Application.BackgroundJobs;
using CheckDrive.Application.Validators.Car;
using CheckDrive.Application.Mappings;

namespace CheckDrive.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection RegisterApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(CarMappings).Assembly);
        services.AddValidatorsFromAssemblyContaining<CreateCarValidator>();

        AddServices(services);

        return services;
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IDoctorReviewService, DoctorReviewService>();
        services.AddScoped<IMechanicHandoverService, MechanicHandoverService>();
        services.AddScoped<IOperatorReviewService, OperatorReviewService>();
        services.AddScoped<IMechanicAcceptanceService, MechanicAcceptanceService>();
        services.AddScoped<IDispatcherReviewService, DispatcherReviewService>();
        services.AddScoped<IManagerReviewService, ManagerReviewService>();
        services.AddScoped<ICheckPointService, CheckPointService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IDriverService, DriverService>();
        services.AddScoped<ICarService, CarService>();
        services.AddScoped<IOilMarkService, OilMarkService>();
        services.AddScoped<IReviewHistoryService, ReviewHistoryService>();

        services.AddHostedService<ResetCarLimitsService>();
        services.AddHostedService<ResetDriverStatusService>();
    }
}
