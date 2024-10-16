using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu;
using MNX.MonitoringCenter.Inventory.DataAccess.Rigs;
using System.Reflection;

namespace MNX.MonitoringCenter.Inventory.DataAccess;

/// <summary>
/// Контекст базы данных.
/// </summary>
public class Context : DbContext
{
    /// <summary>
    /// Риги.
    /// </summary>
    internal DbSet<RigDto> Rigs { get; set; }

    /// <summary>
    /// Записи инвентаризаций.
    /// </summary>
    internal DbSet<RigInventory.RigInventory> RigInventory { get; set; }

    /// <summary>
    /// Видеокарты.
    /// </summary>
    internal DbSet<Gpu> Gpu { get; set; }

    public Context(DbContextOptions<Context> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
