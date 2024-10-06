using CheckDrive.Api.Filters;
using CheckDrive.Application.Extensions;
using CheckDrive.Application.Validators;
using CheckDrive.Infrastructure.Configurations;
using CheckDrive.Infrastructure.Extensions;
using CheckDrive.TestDataCreator.Configurations;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Text;
using CheckDrive.Api.Filters;

namespace CheckDrive.Api.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterApplication();
        services.RegisterInfrastructure(configuration);

        services.AddSingleton<FileExtensionContentTypeProvider>();

        AddControllers(services);
        AddSwagger(services);
        AddAuthentication(services, configuration);
        AddAuthorization(services);
        AddConfigurationOptiosn(services, configuration);
        AddSyncfusion(configuration);
        AddValidators(services);

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

        services.AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters();
    }

    private static void AddSwagger(IServiceCollection services)
    {
        services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc("v1", new OpenApiInfo { Title = "Check-Drive API", Version = "v1" });

                setup.SchemaFilter<EnumSchemaFilter>();
            });
    }

    private static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

        services
            .AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;

                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtOptions!.SecretKey))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["tasty-cookies"];

                        return Task.CompletedTask;
                    }
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
    public static void AddValidators(IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CreateMechanicHandoverReviewDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<CreateOperatorReviewDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<CreateMechanicAcceptanceReviewDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<CreateDispatcherReviewDtoValidator>();
    }
}

