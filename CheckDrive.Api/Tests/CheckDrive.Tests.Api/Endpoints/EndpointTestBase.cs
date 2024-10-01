using AutoFixture;
using Bogus;
using CheckDrive.Domain.Common;
using CheckDrive.Infrastructure.Persistence;
using CheckDrive.Tests.Api.Extensions;
using CheckDrive.Tests.Api.Helpers;
using CheckDrive.Tests.Api.ResponseValidators;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using Xunit.Abstractions;

namespace CheckDrive.Tests.Api.Endpoints;

[Collection(nameof(DatabaseCollection))]
public class EndpointTestBase : IClassFixture<TestingWebApplicationFactory>
{
    protected readonly TestingWebApplicationFactory _factory;
    protected readonly ITestOutputHelper _outputHelper;
    protected readonly ResponseValidator _responseValidator;
    protected readonly CheckDriveDbContext _context;
    protected readonly Fixture _fixture;
    protected readonly Faker _faker;
    protected readonly ApiClient _client;

    public EndpointTestBase(TestingWebApplicationFactory factory, ITestOutputHelper outputHelper)
    {
        factory.ClientOptions.BaseAddress = new Uri("https://localhost/api/");
        factory.ClientOptions.AllowAutoRedirect = false;
        _factory = factory;

        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        _outputHelper = outputHelper;
        _responseValidator = factory.ResponseValidator;
        _context = factory.Context;
        _fixture = CreateFixture();
        _faker = new Faker();
        _client = new ApiClient(client, outputHelper);
    }

    public static readonly IEnumerable<object[]> InvalidIds = [[-1], [0]];

    protected static class Routes
    {
        public const string Category = "categories";
        public const string Product = "products";
        public const string Supplier = "suppliers";
        public const string Supply = "supplies";
    }

    protected async Task<int> GetRandomIdAsync<T>() where T : EntityBase
    {
        var ids = await _context.Set<T>()
            .AsNoTracking()
            .Select(x => x.Id)
            .ToListAsync();
        var randomId = ids.GetRandomElement();

        return randomId;
    }

    protected async Task<T> GetRandomEntityAsync<T>() where T : EntityBase
    {
        var elements = await _context.Set<T>().ToListAsync();
        var randomElement = elements.GetRandomElement();

        return randomElement;
    }

    /// <summary>
    /// Returns an Id that does not exist in table belonging to <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>Id not existing in the table.</returns>
    protected async Task<int> GetNonExistingIdAsync<T>() where T : EntityBase
    {
        var maxId = await _context.Set<T>().MaxAsync(x => x.Id);
        var invalidId = maxId + _fixture.Create<int>();

        return invalidId;
    }

    protected static string GetNotFoundErrorMessage(int id, string typeName)
    {
        return $"{typeName} with id: {id} is not found.";
    }

    private static Fixture CreateFixture()
    {
        var fixture = new Fixture();
        fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        fixture.Behaviors.Add(new NullRecursionBehavior());

        return fixture;
    }
}
