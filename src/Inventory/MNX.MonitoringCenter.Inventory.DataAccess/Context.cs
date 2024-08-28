using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace MNX.MonitoringCenter.Inventory.DataAccess;

/// <summary>
/// Контекст базы данных.
/// </summary>
public class Context : DbContext
{
    /// <summary>
    /// Записи инвентаризаций.
    /// </summary>
    internal DbSet<Inventory> Inventory { get; set; }

    public Context(DbContextOptions<Context> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
