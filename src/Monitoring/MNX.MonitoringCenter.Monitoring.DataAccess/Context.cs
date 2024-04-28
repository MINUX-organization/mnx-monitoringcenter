using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Monitoring.DataAccess.Dto;
using System.Reflection;

namespace MNX.MonitoringCenter.Monitoring.DataAccess;

/// <summary>
/// Контекст базы данных.
/// </summary>
public class Context : DbContext
{
    /// <summary>
    /// Риги.
    /// </summary>
    public DbSet<RigDto> Rigs { get; set; }

    public Context(DbContextOptions<Context> option) : base(option) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}