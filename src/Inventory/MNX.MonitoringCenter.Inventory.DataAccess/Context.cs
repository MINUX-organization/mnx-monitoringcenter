using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Inventory.DataAccess.Rigs;
using MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Entities;
using MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Views;

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
    internal DbSet<Rigs.RigInventory> RigInventory { get; set; }

    /// <summary>
    /// Видеокарты.
    /// </summary>
    internal DbSet<GpuInventory> Gpu { get; set; }

    /// <summary>
    /// Нематериализованное представление инвентаризации видеокарт с
    /// наименованием ригов и версиями драйверов.
    /// </summary>
    internal DbSet<GpuInventoryView> GpuViews { get; set; } 
    
    /// <summary>
    /// Нематериализованное представление инвентаризации ограничений видеокарт
    /// с идентификатором инвентаризации рига и производителем видеокарты.
    /// </summary>
    internal DbSet<GpuRestrictionsView> GpuRestrictionsView { get; set; }

    ///
    public Context(DbContextOptions<Context> options) : base(options) { }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
