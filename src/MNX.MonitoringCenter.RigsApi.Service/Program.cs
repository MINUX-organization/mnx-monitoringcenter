using Microsoft.AspNetCore.Authentication.JwtBearer;
using MNX.Application.Consul;
using MNX.MonitoringCenter.Infrastructure;
using MNX.MonitoringCenter.Inventory.Integration;
using MNX.SecurityManagement.Authentication.Integration;
using NLog;
using NLog.Web;
using System.Reflection;

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

        services.AddSignalR();

        services.AddScoped<UserAccessor>();
        services.AddHttpContextAccessor();
    }

    private static async Task RunApp(WebApplicationBuilder builder)
    {
        var app = builder.Build();
        var appName = builder.Configuration["ServiceName"]
            ?? throw new ArgumentNullException(null, "Не указано название сервиса");

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

        await app.RunAsync();
    }
}
