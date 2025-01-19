using CheckDrive.Api.Extensions;
using CheckDrive.Application.Hubs;
using Microsoft.AspNetCore.CookiePolicy;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

builder.Host.UseSerilog(
    (context, _, configuration) =>
    {
        configuration.ReadFrom.Configuration(context.Configuration);
    });

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

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

app.Run();

// For API testing
public partial class Program { }