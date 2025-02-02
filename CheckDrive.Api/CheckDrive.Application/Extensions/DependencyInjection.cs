using CheckDrive.Application.Interfaces;
using CheckDrive.Application.Interfaces.Auth;
using CheckDrive.Application.Interfaces.Jobs;
using CheckDrive.Application.Interfaces.Reports;
using CheckDrive.Application.Interfaces.Review;
using CheckDrive.Application.Mappings;
using CheckDrive.Application.Services;
using CheckDrive.Application.Services.Auth;
using CheckDrive.Application.Services.Jobs;
using CheckDrive.Application.Services.Reports;
using CheckDrive.Application.Services.Review;
using CheckDrive.Application.Validators.Car;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CheckDrive.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection RegisterApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(CarMappings).Assembly);
        services.AddValidatorsFromAssemblyContaining<CreateCarValidator>();

        AddServices(services);
        AddBackgroundJobs(services);

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
        services.AddScoped<IDebtService, DebtService>();
        services.AddScoped<ICarService, CarService>();
        services.AddScoped<IOilMarkService, OilMarkService>();
        services.AddScoped<IReviewHistoryService, ReviewHistoryService>();
        services.AddScoped<ITokenHandler, TokenHandler>();
        services.AddScoped<IReportService, ReportService>();
    }

    private static void AddBackgroundJobs(IServiceCollection services)
    {
        services.AddScoped<IResetCarLimitsService, ResetCarLimitsService>();
        services.AddScoped<IResetDriverStatusService, ResetDriverStatusService>();
    }
}
