using CheckDrive.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;

namespace CheckDrive.Tests.Api.Helpers;

public class DatabaseFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _sqlServerContainer = new MsSqlBuilder()
        .WithName("TestDb")
        .Build();

    private CheckDriveDbContext? _context;
    public CheckDriveDbContext Context
    {
        get
        {
            if (_context is null)
            {
                var options = new DbContextOptionsBuilder<CheckDriveDbContext>()
                    .UseSqlServer(SqlServerConnectionString)
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                    .Options;

                _context = new CheckDriveDbContext(options);
            }

            return _context;
        }
    }

    public string SqlServerConnectionString
    {
        get
        {
            var connectionString = new SqlConnectionStringBuilder(_sqlServerContainer.GetConnectionString())
            {
                InitialCatalog = "TestDb"
            };

            return connectionString.ConnectionString;
        }
    }

    public async Task InitializeAsync()
    {
        try
        {
            _context = await GetSqlServerAsync();

            await _context.Database.EnsureDeletedAsync();
            await _context.Database.EnsureCreatedAsync();
            await _context.Database.OpenConnectionAsync();
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("There was an error intializing test database, {0}", ex.Message);
            throw;
        }
    }

    public async Task DisposeAsync()
    {
        if (_context != null)
        {
            await _context.Database.EnsureDeletedAsync();
        }

        if (_sqlServerContainer != null)
        {
            await _sqlServerContainer.DisposeAsync();
        }
    }

    private async Task<CheckDriveDbContext> GetSqlServerAsync()
    {
        await _sqlServerContainer.StartAsync();

        var options = new DbContextOptionsBuilder<CheckDriveDbContext>()
            .UseSqlServer(SqlServerConnectionString)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .Options;

        var context = new CheckDriveDbContext(options);

        return context;
    }
}

[CollectionDefinition(nameof(DatabaseCollection))]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}
