using Microsoft.AspNetCore.Authentication.JwtBearer;
using MNX.Application.Consul;
using MNX.Application.Data.DI;
using MNX.Application.RabbitMQ;
using MNX.MonitoringCenter.Infrastructure;
using MNX.MonitoringCenter.Monitoring.DataAccess;
using MNX.MonitoringCenter.Monitoring.DataAccess.Repositories;
using MNX.MonitoringCenter.Monitoring.DataAccess.Repositories.MiningDevices;
using MNX.MonitoringCenter.Monitoring.Hubs;
using MNX.MonitoringCenter.Monitoring.Service.Infrastructure;
using MNX.MonitoringCenter.Monitoring.UseCases.Abstractions;
using MNX.MonitoringCenter.Monitoring.UseCases.Queries.GetRigsInformation;
using MNX.SecurityManagement.Authentication.Integration;
using NLog;
using NLog.Web;
using System.Reflection;
using ZiggyCreatures.Caching.Fusion;

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
            logger.Error(ex, "Произошла ошибка при запуске хоста");
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

        services.AddControllers();
        services.AddConsulIntegration(builder.Configuration);
        services.AddHealthChecks();

        var basePath = AppContext.BaseDirectory;
        var xmlFilePath = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        services.AddSwagger(Path.Combine(basePath, xmlFilePath));

        services.AddJwtBearerAuthentication(builder.Configuration["SecretKey"]!, new JwtBearerEvents()
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                var path = context.HttpContext.Request.Path;

                if (!string.IsNullOrEmpty(accessToken)
                && path.StartsWithSegments("hubs"))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        });

        ConfigureDI(services, builder.Configuration);

        return builder;
    }

    private static void ConfigureDI(IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddDataContext<Context>(configuration);
        services.AddSignalR();
        services.AddEasyNetQ(configuration, [Assembly.GetExecutingAssembly()]);
        services.AddMemoryCache()
                .AddFusionCache()
                .WithDefaultEntryOptions(options => options.Duration = TimeSpan.FromMinutes(15)); // todo: вынести настройку в конфиг

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(new Assembly[]
        {
            Assembly.GetExecutingAssembly(),
            typeof(GetRigsInformationQuery).Assembly
        }));

        services.AddAutoMapper(new Assembly[]
        {
            typeof(MappingProfile).Assembly,
            typeof(MNX.MonitoringCenter.Monitoring.UseCases.MappingProfile).Assembly,
            typeof(DbMappingProfile).Assembly
        });

        services.AddScoped<IRigRepository, RigRepository>();
        services.AddScoped<IMiningDeviceRepository, MiningDeviceRepository>();
        services.AddScoped<IGpuRepository, GpuRepository>();
        services.AddScoped<UserAccessor>();
        services.AddSingleton<IUserRigsObserverWrapper, UserRigsObserverWrapper>();
        services.AddHttpContextAccessor();

        services.Configure<DynamicDataOptions>(configuration);
    }

    private static async Task RunApp(WebApplicationBuilder builder)
    {
        var app = builder.Build();
        var appName = builder.Configuration["ServiceName"]
            ?? throw new ArgumentNullException(null, "Не указано название сервиса");

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapHealthChecks("/health").AllowAnonymous();
        app.MapGet(string.Empty, async ctx => await ctx.Response.WriteAsync(appName)).AllowAnonymous();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapHub<MonitoringHub>("hubs/monitoring");
        app.MapControllers();

        await app.RunAsync();
    }
}