using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.Core.FlightSheet;
using System.Reflection;

namespace MNX.MonitoringCenter.Management.DataAccess;

/// <summary>
/// Контекст БД.
/// </summary>
public class Context : DbContext
{
    internal DbSet<Core.Cryptocurrency> Cryptocurrencies { get; set; }

    internal DbSet<FlightSheetBase> FlightSheets { get; set; }

    internal DbSet<FlightSheetConfig> FlightSheetConfigs { get; set; }

    internal DbSet<Core.Wallet> Wallets { get; set; }

    internal DbSet<Core.Pool> Pools { get; set; }

    internal DbSet<Core.Algorithm> Algorithms { get; set; }

    internal DbSet<Core.Miner> Miners { get; set; }

    internal DbSet<Core.Preset> Presets { get; set; }

    internal DbSet<Overclocking> Overclocking { get; set; }

    public Context(DbContextOptions<Context> option) : base(option) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}