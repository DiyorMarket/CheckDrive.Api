﻿using CheckDrive.Application.Configurations;
using CheckDrive.Application.Extensions;
using CheckDrive.Infrastructure.Extensions;
using CheckDrive.TestDataCreator.Configurations;
using CheckDrive.TestDataCreator.Extensions;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Text;

namespace CheckDrive.Api.Extensions;

internal static class DependencyInjection
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterApplication();
        services.RegisterInfrastructure(configuration);
        services.RegisterTestDataCreator();

        services.AddSingleton<FileExtensionContentTypeProvider>();
        services.AddSignalR(options => options.EnableDetailedErrors = true);
        services.AddHttpContextAccessor();

        AddControllers(services);
        AddSwagger(services);
        AddAuthentication(services, configuration);
        AddAuthorization(services);
        AddConfigurationOptiosn(services, configuration);
        AddSyncfusion(configuration);
        AddHangfire(services, configuration);

        return services;
    }

    private static void AddControllers(IServiceCollection services)
    {
        services
            .AddControllers(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
                options.ReturnHttpNotAcceptable = true;
            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            })
            .AddXmlSerializerFormatters();
    }

    private static void AddSwagger(IServiceCollection services)
    {
        services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc("v1", new OpenApiInfo { Title = "Check-Drive API", Version = "v1" });
            })
            .AddSwaggerGenNewtonsoftSupport();
    }

    private static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>()
            ?? throw new InvalidOperationException("Could not load JWT configurations.");

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                };
            });
    }

    private static void AddAuthorization(IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy("Admin", policy =>
            {
                policy.RequireClaim("Admin", "true");
            })
            .AddPolicy("AdminOrDriver", policy =>
            {
                policy.RequireAssertion(context =>
                    context.User.HasClaim(c => c.Type == "Driver" && c.Value == "true") ||
                    context.User.HasClaim(c => c.Type == "Admin" && c.Value == "true"));
            })
            .AddPolicy("AdminOrDoctor", policy =>
            {
                policy.RequireAssertion(context =>
                    context.User.HasClaim(c => c.Type == "Doctor" && c.Value == "true") ||
                    context.User.HasClaim(c => c.Type == "Admin" && c.Value == "true"));
            })
            .AddPolicy("AdminOrOperator", policy =>
            {
                policy.RequireAssertion(context =>
                    context.User.HasClaim(c => c.Type == "Operator" && c.Value == "true") ||
                    context.User.HasClaim(c => c.Type == "Admin" && c.Value == "true"));
            })
            .AddPolicy("AdminOrDispatcher", policy =>
            {
                policy.RequireAssertion(context =>
                    context.User.HasClaim(c => c.Type == "Dispatcher" && c.Value == "true") ||
                    context.User.HasClaim(c => c.Type == "Admin" && c.Value == "true"));
            })
            .AddPolicy("AdminOrMechanic", policy =>
            {
                policy.RequireAssertion(context =>
                    context.User.HasClaim(c => c.Type == "Mechanic" && c.Value == "true") ||
                    context.User.HasClaim(c => c.Type == "Admin" && c.Value == "true"));
            });
    }

    private static void AddConfigurationOptiosn(IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<DataSeedOptions>()
            .Bind(configuration.GetSection(DataSeedOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<HangfireSettings>()
            .Bind(configuration.GetSection(HangfireSettings.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }

    private static void AddSyncfusion(IConfiguration configuration)
    {
        var key = configuration.GetValue<string>("SyncfusionKey");

        if (string.IsNullOrEmpty(key))
        {
            throw new InvalidOperationException("Syncfusion key is not found in configurations.");
        }

        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(key);
    }

    private static void AddHangfire(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Cannot setup Hangfire without 'Connection String'");

        services.AddHangfire(options =>
        {
            options.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseDefaultTypeSerializer()
                .UseSqlServerStorage(connectionString);
        });

        services.AddHangfireServer();
    }
}

