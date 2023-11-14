using MINUX.Backend.Unit.DataAccess;
using MINUX.Backend.Unit.DataAccess.Repositories;
using MINUX.Backend.Unit.UseCases.Abstractions;
using MINUX.Backend.Unit.UseCases.Commands.DeleteCommand;

namespace MINUX.Backend.Unit;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DeleteCommandHandler).Assembly));
        builder.Services.AddScoped<IMainRepository, MainRepository>();
        builder.Services.AddDbContext<Context>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}