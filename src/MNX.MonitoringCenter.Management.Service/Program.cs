using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Management.DataAccess;
using MNX.MonitoringCenter.Management.DataAccess.Repositories;
using MNX.MonitoringCenter.Management.UseCases;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;
using MNX.MonitoringCenter.Management.UseCases.Queries.GetAlgorithmsQuery;
using Kernel.UseCases.DI;
using NLog;
using NLog.Web;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets.SavePreset;
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

        services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            var basePath = AppContext.BaseDirectory;
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(basePath, xmlFile);
            options.IncludeXmlComments(xmlPath);
        });

        services.AddValidationPipelines(typeof(SavePresetValidator).Assembly);
        services.AddHealthChecks();

        ConfigureDI(services, builder.Configuration);

        return builder;
    }

    private static void ConfigureDI(IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddAutoMapper(cfg => cfg.AddProfile(typeof(MappingProfile)));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAvailableAlgorithmsQuery).Assembly));
        services.AddDbContext<Context>(options => options.UseSqlite("Data Source = Minux.db"));
        services.AddMemoryCache();

        var monitoringUri = configuration["MonitoringUri"]
            ?? throw new ArgumentNullException("MonitoringUri", "Uri адрес сервиса мониторинга не указан");
        services.AddRefitClient<IMonitoringClient>()
                .ConfigureHttpClient(client => client.BaseAddress = new Uri(monitoringUri));

        services.AddScoped<IAlgorithmRepository, AlgorithmRepository>();
        services.AddScoped<ICryptocurrencyRepository, CryptocurrencyRepository>();
        services.AddScoped<IFlightSheetRepository, FlightSheetRepository>();
        services.AddScoped<IMinerRepository, MinerRepository>();
        services.AddScoped<IPoolRepository, PoolRepository>();
        services.AddScoped<IPresetRepository, PresetRepository>();
        services.AddScoped<IWalletRepository, WalletRepository>();
    }

    private static async Task RunApp(WebApplicationBuilder builder)
    {
        var app = builder.Build();
        var appName = builder.Configuration["ServiceName"]
            ?? throw new ArgumentNullException("ServiceName", "Не указано название сервиса");

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllers();
        app.MapHealthChecks("/health");
        app.MapGet(string.Empty, async ctx => await ctx.Response.WriteAsync(appName));

        await app.RunAsync();
    }
}