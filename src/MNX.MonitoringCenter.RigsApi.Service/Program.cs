using NLog;
using NLog.Web;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MNX.Application.Consul;
using MNX.Application.OpenTelemetry;
using MNX.Application.CustomMiddlewares;
using MNX.Application.OpenTelemetry.Metrics;
using MNX.Application.OpenTelemetry.Tracing;
using MNX.MonitoringCenter.RigsApi.Streams;
using MNX.MonitoringCenter.Traffic.Integration;
using MNX.MonitoringCenter.RigsApi.Service.Hubs;
using MNX.MonitoringCenter.Inventory.Integration;
using MNX.MonitoringCenter.Management.Integration;
using MNX.Application.Bus.RabbitMQ.Agent.Extensions;
using MNX.MonitoringCenter.RigsApi.Service.Consumers;
using MNX.SecurityManagement.Authentication.Integration;
using MNX.MonitoringCenter.RigsApi.Service.EventHandlers;
using MNX.MonitoringCenter.RigsApi.Service.Infrastructure;
using MNX.MonitoringCenter.RigsApi.Service.Hubs.Notification;
using MNX.MonitoringCenter.RigsApi.UnionStreams.Abstractions;
using MNX.MonitoringCenter.RigsApi.UseCases.Devices.Queries.GetGpus;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;

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
        var configuration = builder.Configuration;

        services.AddControllers()
                .AddJsonOptions(options =>
                {
                    // <НЕ ПЕРЕСТАВЛЯТЬ>
                    options.JsonSerializerOptions.Converters.Add(new EnumFlagsConverter());
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    // </НЕ ПЕРЕСТАВЛЯТЬ>

                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic);
                });

        services.AddConsulIntegration(builder.Configuration);

        services.ConfigureOpenTelemetry(configuration)
                .ConfigureTracing(configuration)
                .ConfigureMetrics(configuration);

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

        services.AddJwtBearerAuthentication(configuration["SecretKey"]!, new JwtBearerEvents()
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                var path = context.HttpContext.Request.Path;

                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
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

        services.AddEasyNetQWithAgentMsgSupport(configuration, new Assembly[]
        {
            typeof(AgentLifeCycleMsgConsumer).Assembly
        });

        services.AddHostedService<RigDynamicIndicatorsConsumer>();

        services.AddMediatR(x => x.RegisterServicesFromAssemblies(
            typeof(GetGpusQuery).Assembly,
            typeof(RigInventorySavedEventHandler).Assembly
            ));

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

        app.UseExceptionHandlingMiddleware();
        
        app.MapControllers();

        app.MapHub<MonitoringHub>("hubs/monitoring");
        app.MapHub<NotificationHub>("hubs/notification");

        return app.RunAsync();
    }
}
