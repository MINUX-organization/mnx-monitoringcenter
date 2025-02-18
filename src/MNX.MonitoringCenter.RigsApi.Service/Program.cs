using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using MNX.Application.Consul;
using MNX.Application.RabbitMQ;
using MNX.MonitoringCenter.Inventory.Integration;
using MNX.MonitoringCenter.Management.Integration;
using MNX.MonitoringCenter.RigsApi.Service.Consumers;
using MNX.MonitoringCenter.RigsApi.Service.Hubs;
using MNX.MonitoringCenter.RigsApi.Service.Infrastructure;
using MNX.MonitoringCenter.RigsApi.Streams;
using MNX.MonitoringCenter.RigsApi.UnionStreams;
using MNX.MonitoringCenter.RigsApi.UnionStreams.Abstractions;
using MNX.MonitoringCenter.RigsApi.UseCases.Devices.Queries.GetGpus;
using MNX.MonitoringCenter.Traffic.Controllers;
using MNX.MonitoringCenter.Traffic.Integration;
using MNX.SecurityManagement.Authentication.Integration;
using NLog;
using NLog.Web;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace MNX.MonitoringCenter.RigsApi.Service;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
        try
        {
            logger.Debug("init main");
            var builder = ConfigureApp(args);
            await RunApp(builder);
        }
        catch (Exception ex)
        {
            logger.Error(ex, "An error occurred while starting the host");
            throw;
        }
        finally
        {
            LogManager.Shutdown();
        };
    }

    private static WebApplicationBuilder ConfigureApp(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Logging.ClearProviders();
        builder.Host.UseNLog();
        var services = builder.Services;

        services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic);
                });

        services.AddConsulIntegration(builder.Configuration);

        services.AddEndpointsApiExplorer();

        var basePath = AppContext.BaseDirectory;
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        services.AddSwaggerGen(opts =>
        {
            opts.IncludeXmlComments(Path.Combine(basePath, xmlFile), includeControllerXmlComments: true);

            opts.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Enter access token",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            opts.AddSecurityRequirement(new OpenApiSecurityRequirement {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                },
                Array.Empty<string>()
            } });

            opts.EnableAnnotations();
            opts.UseAllOfToExtendReferenceSchemas();
            opts.UseAllOfForInheritance();
            opts.UseOneOfForPolymorphism();
            opts.UseInlineDefinitionsForEnums();

            opts.SelectDiscriminatorNameUsing(_ => "$type");

            var allTypes = AppDomain.CurrentDomain.GetAssemblies()
                                                  .Where(a => !a.IsDynamic)
                                                  .SelectMany(a => a.GetTypes());

            // Swashbuckle работает только с классами, как с базовыми типами, и не работает с интерфейсами
            opts.SelectSubTypesUsing(baseType =>
            {
                if (baseType.IsInterface)
                {
                    return allTypes.Where(t => t.GetInterfaces()
                                                .Where(i => i.GetCustomAttributes<JsonDerivedTypeAttribute>()
                                                .Any(x => x.DerivedType == t)).Any());
                }

                return allTypes.Where(t => t.IsSubclassOf(baseType));
            });

            
        });

        services.AddJwtBearerAuthentication(builder.Configuration["SecretKey"]!, new JwtBearerEvents()
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                var path = context.HttpContext.Request.Path;

                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("hubs"))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        });

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        ConfigureDI(services, builder.Configuration);

        return builder;
    }

    private static void ConfigureDI(IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddInventoryModule(configuration);
        services.AddManagementModule(configuration);
        services.AddTrafficProcessing(configuration);

        services.AddScoped<IUnionStreamBuilder, UnionStreamBuilder>();

        services.AddEasyNetQ(configuration, new Assembly[]
        {
            typeof(RigsDynamicIndicatorsConsumer).Assembly,
            typeof(RigConsumer).Assembly
        });

        services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(GetGpusQuery).Assembly));

        services.AddSignalR();

        services.AddScoped<UserAccessor>();
        services.AddHttpContextAccessor();
    }

    private static Task RunApp(WebApplicationBuilder builder)
    {
        var app = builder.Build();
        var appName = builder.Configuration["ServiceName"]
            ?? throw new ArgumentNullException(null, "Service name not specified");

        //if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();
        app.UseCors();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapHealthChecks("/health").AllowAnonymous();
        app.MapGet(string.Empty, async ctx => await ctx.Response.WriteAsync(appName)).AllowAnonymous();
        app.MapControllers();

        app.MapHub<MonitoringHub>("hubs/monitoring");

        return app.RunAsync();
    }
}
