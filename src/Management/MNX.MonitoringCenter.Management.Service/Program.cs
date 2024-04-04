using MNX.Application.Consul;
using MNX.Application.Data.DI;
using MNX.Application.UseCases.DI;
using MNX.MonitoringCenter.Infrastructure;
using MNX.MonitoringCenter.Management.DataAccess;
using MNX.MonitoringCenter.Management.DataAccess.Repositories;
using MNX.MonitoringCenter.Management.UseCases;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets.SavePreset;
using MNX.MonitoringCenter.Management.UseCases.Queries.GetAlgorithmsQuery;
using MNX.SecurityManagement.Authentication.Integration;
using NLog;
using NLog.Web;
using Refit;
using System.Reflection;

namespace MNX.MonitoringCenter.Management;

public class Program
{
    public static async Task Main(string[] args)
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
        }
    }

    private static WebApplicationBuilder ConfigureApp(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Logging.ClearProviders();
        builder.Host.UseNLog();

        var services = builder.Services;

        services.AddConsulIntegration(builder.Configuration);

        services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();

        var basePath = AppContext.BaseDirectory;
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        services.AddSwagger(Path.Combine(basePath, xmlFile));

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        services.AddHealthChecks();

        services.AddJwtBearerAuthentication(builder.Configuration["SecretKey"]!);

        ConfigureDI(services, builder.Configuration);

        return builder;
    }

    private static void ConfigureDI(IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddAutoMapper(cfg => cfg.AddProfile(typeof(MappingProfile)));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAvailableAlgorithmsQuery).Assembly));
        services.AddValidationPipelines(typeof(SavePresetValidator).Assembly);
        services.AddDataContext<Context>(configuration);

        var monitoringUri = configuration["MonitoringUri"]
            ?? throw new ArgumentNullException(null, "Uri адрес сервиса мониторинга не указан");
        services.AddRefitClient<IMonitoringClient>()
                .ConfigureHttpClient(client => client.BaseAddress = new Uri(monitoringUri));

        services.AddScoped<IAlgorithmRepository, AlgorithmRepository>();
        services.AddScoped<ICryptocurrencyRepository, CryptocurrencyRepository>();
        services.AddScoped<IFlightSheetRepository, FlightSheetRepository>();
        services.AddScoped<IMinerRepository, MinerRepository>();
        services.AddScoped<IPoolRepository, PoolRepository>();
        services.AddScoped<IPresetRepository, PresetRepository>();
        services.AddScoped<IWalletRepository, WalletRepository>();
        services.AddScoped<UserAccessor>();
        services.AddHttpContextAccessor();
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

        app.UseRouting();
        app.UseCors();

        app.MapHealthChecks("/health").AllowAnonymous();
        app.MapGet(string.Empty, async ctx => await ctx.Response.WriteAsync(appName)).AllowAnonymous();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        await app.RunAsync();
    }
}