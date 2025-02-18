using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Management.Core.Mining.Miner;
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Configs;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice;
using MNX.MonitoringCenter.Management.DataAccess.FlightSheet.Dto;
using MNX.MonitoringCenter.Management.DataAccess.FlightSheet.Dto.Target;
using MNX.MonitoringCenter.Management.DataAccess.Overclocking;
using System.Reflection;

namespace MNX.MonitoringCenter.Management.DataAccess;

/// <summary>
/// Контекст БД.
/// </summary>
public class Context : DbContext
{
    internal DbSet<Core.Mining.Algorithm> Algorithms { get; set; }

    internal DbSet<Core.Mining.Cryptocurrency> Cryptocurrencies { get; set; }

    internal DbSet<FlightSheetDto> FlightSheets { get; set; }

    internal DbSet<BaseFlightSheetTargetDto> FlightSheetTargets { get; set; }

    internal DbSet<MiningCoinConfig> MiningCoinConfigs { get; set; }

    internal DbSet<Core.Mining.Miner.Miner> Miners { get; set; }

    internal DbSet<MinerAlgorithm> MinerAlgorithms { get; set; }

    internal DbSet<MiningDeviceInfo> MiningDevices { get; set; }

    internal DbSet<Core.Mining.Pool> Pools { get; set; }

    internal DbSet<Core.Overclocking.Preset> Presets { get; set; }

    internal DbSet<OverclockingDto> Overclocking { get; set; }

    internal DbSet<Core.Mining.Wallet> Wallets { get; set; }

    public Context(DbContextOptions<Context> option) : base(option) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}