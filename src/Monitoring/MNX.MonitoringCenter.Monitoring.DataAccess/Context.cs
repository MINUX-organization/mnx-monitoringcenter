using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Monitoring.Core;

namespace MNX.MonitoringCenter.Monitoring.DataAccess;

/// <summary>
/// Контекст базы данных.
/// </summary>
public class Context : DbContext
{
    /// <summary>
    /// Риги.
    /// </summary>
    public DbSet<Rig> Rigs { get; set; }

    public Context(DbContextOptions<Context> option) : base(option) { }
}