using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.Core.FlightSheet.Target;
using System.Reflection;
using MNX.MonitoringCenter.Management.Core.Enums;

namespace MNX.MonitoringCenter.Management.DataAccess;

/// <summary>
/// Контекст БД.
/// </summary>
public class Context : DbContext
{
    internal DbSet<Core.Algorithm> Algorithms { get; set; }

    internal DbSet<Core.Cryptocurrency> Cryptocurrencies { get; set; }

    internal DbSet<Core.FlightSheet.FlightSheet> FlightSheets { get; set; }

    internal DbSet<FlightSheetTargetBase> FlightSheetTargets { get; set; }

    internal DbSet<FlightSheetTargetConfig> FlightSheetTargetConfigs { get; set; }

    internal DbSet<Core.Miner> Miners { get; set; }

    internal DbSet<Core.Pool> Pools { get; set; }

    internal DbSet<Core.Preset> Presets { get; set; }

    internal DbSet<Overclocking> Overclocking { get; set; }

    internal DbSet<Core.Wallet> Wallets { get; set; }

    public Context(DbContextOptions<Context> option) : base(option) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.Entity<Core.Algorithm>().HasData(new Core.Algorithm { Name = "Algorithm" });
        modelBuilder.Entity<Core.Miner>().HasData(
            new Core.Miner
            {
                Id = Guid.Parse("251752F8-419A-4EB3-8631-50A7557CFF7B"), 
                Name = "Miner", 
                Version = "1.0", 
                MiningMode = GpuMiningModeEnum.Dual, 
                SupportedDevices = DeviceEnum.AmdGpu | DeviceEnum.IntelGpu | DeviceEnum.NVidiaGpu
            });
    }
}