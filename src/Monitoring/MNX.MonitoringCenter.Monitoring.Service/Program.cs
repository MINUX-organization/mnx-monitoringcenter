using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MNX.Infrastructure.RabbitMQ;
using MNX.MonitoringCenter.Monitoring.DataAccess;
using MNX.MonitoringCenter.Monitoring.DataAccess.Repositories;
using MNX.MonitoringCenter.Monitoring.Hubs;
using MNX.MonitoringCenter.Monitoring.UseCases;
using MNX.MonitoringCenter.Monitoring.UseCases.Abstractions;
using NLog;
using NLog.Web;
using System.Reflection;
using System.Text;

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

        ConfigureDI(builder.Services, builder.Configuration);

        return builder;
    }

    private static void ConfigureDI(IServiceCollection services, ConfigurationManager configuration)
    {
        // TODO: Использовать аутентификацию из nuget-пакета. Сейчас параметры установлены ради тестирования.
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        IssuerSigningKey = new SymmetricSecurityKey
                            (Encoding.UTF8.GetBytes("LDktKdoQak3Pk0cnXxCltA-LDktKdoQak3Pk0cnXxCltA")),
                        // указывает, будет ли валидироваться издатель при валидации токена
                        ValidateIssuer = false,
                        // будет ли валидироваться потребитель токена
                        ValidateAudience = false,
                        // будет ли валидироваться время существования
                        ValidateLifetime = false,
                        // валидация ключа безопасности
                        ValidateIssuerSigningKey = false,
                    };

                    options.Events = new JwtBearerEvents()
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
                    };
                });

        services.AddDbContext<Context>(options => options.UseSqlite("Data Source = Minux.db"));
        services.AddSignalR();
        services.AddEasyNetQ(configuration, new[] { Assembly.GetExecutingAssembly() });
        services.AddScoped<IRigRepository, RigRepository>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
        services.AddAutoMapper(cfg => cfg.AddProfile(typeof(MappingProfile)));
        services.AddSingleton<ConnectionCounter>();
        services.AddHealthChecks();
    }

    private static async Task RunApp(WebApplicationBuilder builder)
    {
        var app = builder.Build();
        var appName = builder.Configuration["ServiceName"]
            ?? throw new ArgumentNullException("ServiceName", "Не указано название сервиса");

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapHealthChecks("/health");
        app.MapGet(string.Empty, async ctx => await ctx.Response.WriteAsync(appName));
        app.MapHub<MonitoringHub>("hubs/monitoring");

        await app.RunAsync();
    }
}