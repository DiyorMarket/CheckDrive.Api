using CheckDrive.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Tests.Api.Helpers;

public class TestingWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly DatabaseFixture _databaseFixture;

    public CheckDriveDbContext Context => _databaseFixture.Context;

    public TestingWebApplicationFactory(DatabaseFixture databaseFixture)
    {
        _databaseFixture = databaseFixture;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var context = services.SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<CheckDriveDbContext>));

            if (context is not null)
            {
                services.Remove(context);
            }

            services.AddDbContext<CheckDriveDbContext>(options =>
            {
                options.UseSqlServer(_databaseFixture.SqlServerConnectionString);
            });
        });

        builder.UseEnvironment("Testing");
    }
}