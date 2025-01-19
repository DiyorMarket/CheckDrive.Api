using CheckDrive.Api.Extensions;
using CheckDrive.Application.Hubs;
using Microsoft.AspNetCore.CookiePolicy;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseSentry(options =>
{
    options.TracesSampleRate = 1.0;
    options.SetBeforeSend((@event, hint) =>
    {
        @event.ServerName = null;
        return @event;
    });
});

builder.Logging.ClearProviders();

builder.Host.UseSerilog(
    (context, _, configuration) =>
    {
        configuration.ReadFrom.Configuration(context.Configuration);
    });

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

app.UseDatabaseSeeder();

app.UseErrorHandler();

app.UseSentryTracing();

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