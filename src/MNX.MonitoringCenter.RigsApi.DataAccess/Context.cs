using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace MNX.MonitoringCenter.RigsApi.DataAccess;

/// <summary>
/// Контекст БД.
/// </summary>
public class Context : DbContext
{
    /// <summary>
    /// Риги.
    /// </summary>
    internal DbSet<RigDto> Rigs { get; set; }

    ///
    public Context(DbContextOptions<Context> option) : base(option) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
