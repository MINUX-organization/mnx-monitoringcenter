using Microsoft.AspNetCore.Authentication.JwtBearer;
using MNX.Application.Consul;
using MNX.Application.Data.DI;
using MNX.Application.RabbitMQ;
using MNX.MonitoringCenter.Infrastructure;
using MNX.MonitoringCenter.Monitoring.DataAccess;
using MNX.MonitoringCenter.Monitoring.DataAccess.Repositories;
using MNX.MonitoringCenter.Monitoring.Hubs;
using MNX.MonitoringCenter.Monitoring.UseCases;
using MNX.MonitoringCenter.Monitoring.UseCases.Abstractions;
using MNX.SecurityManagement.Authentication.Integration;
using NLog;
using NLog.Web;
using System.Reflection;

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

        services.AddConsulIntegration(builder.Configuration);
        services.AddHealthChecks();

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
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
        services.AddAutoMapper(cfg => cfg.AddProfile(typeof(MappingProfile)));

        services.AddScoped<IRigRepository, RigRepository>();
        services.AddSingleton<ConnectionCounter>();
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
        }

        app.MapHealthChecks("/health").AllowAnonymous();
        app.MapGet(string.Empty, async ctx => await ctx.Response.WriteAsync(appName)).AllowAnonymous();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapHub<MonitoringHub>("hubs/monitoring");

        await app.RunAsync();
    }
}