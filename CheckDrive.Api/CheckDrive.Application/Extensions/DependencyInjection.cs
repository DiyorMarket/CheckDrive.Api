using CheckDrive.Application.Interfaces.Review;
using CheckDrive.Application.Interfaces;
using CheckDrive.Application.Services;
using CheckDrive.Application.Services.Review;
using Microsoft.Extensions.DependencyInjection;
using CheckDrive.Application.Interfaces.Auth;
using CheckDrive.Application.Services.Auth;
using CheckDrive.Application.BackgroundJobs;

namespace CheckDrive.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection RegisterApplication(this IServiceCollection services)
    {
        AddServices(services);
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        return services;
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IDoctorReviewService, DoctorReviewService>();
        services.AddScoped<IMechanicHandoverService, MechanicHandoverService>();
        services.AddScoped<IOperatorReviewService, OperatorReviewService>();
        services.AddScoped<IMechanicAcceptanceService, MechanicAcceptanceService>();
        services.AddScoped<IDispatcherReviewService, DispatcherReviewService>();
        services.AddScoped<ICheckPointService, CheckPointService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IDriverService, DriverService>();
        services.AddScoped<ICarService, CarService>();
        services.AddScoped<IOilMarkService, OilMarkService>();

        services.AddHostedService<CarMonthlyMileageResetService>();
    }
}
