using CheckDrive.Api.Extensions;
using Microsoft.AspNetCore.CookiePolicy;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .Enrich.FromLogContext()
    .WriteTo.Console(new CustomJsonFormatter())
    .WriteTo.File(new CustomJsonFormatter(), "logs/logs.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.File(new CustomJsonFormatter(), "logs/error_.txt", Serilog.Events.LogEventLevel.Error, rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

if (!app.Environment.IsProduction())
{
    app.UseDatabaseSeeder();
}

app.UseErrorHandler();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always,
});

await app.UseEnsureRolesCreatedAsync();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
