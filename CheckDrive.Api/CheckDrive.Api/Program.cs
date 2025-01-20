using CheckDrive.Api.Extensions;
using CheckDrive.Application.Hubs;
using Microsoft.AspNetCore.CookiePolicy;
using Serilog;

try
{
    Log.Logger = new LoggerConfiguration()
        .WriteTo.File("Logs/logs_.txt")
        .MinimumLevel.Information() // Set the minimum level for bootstrap logging
        .CreateBootstrapLogger();

    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();

    builder.Host.UseSerilog(
        (context, _, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);
        });

    builder.Services.ConfigureServices(builder.Configuration);

    var app = builder.Build();

    Log.Logger.Information("API is starting...");

    app.UseHangfire();

    app.UseBackgroundJobs();

    app.UseDatabaseSeeder();

    app.UseErrorHandler();

    app.UseSerilogRequestLogging();

    app.UseSwagger();

    app.UseSwaggerUI();

    app.UseHttpsRedirection();

    app.UseCookiePolicy(new CookiePolicyOptions
    {
        MinimumSameSitePolicy = SameSiteMode.Strict,
        HttpOnly = HttpOnlyPolicy.Always,
        Secure = CookieSecurePolicy.Always,
    });

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.MapHub<ReviewHub>("/review-hub");

    Log.Logger.Information("API is running...");

    app.Run();
}
catch (Exception ex)
{
    Log.Logger.Error(ex, "Error occurred while starting API. {Message}", ex.Message);
}

// For API testing
public partial class Program { }