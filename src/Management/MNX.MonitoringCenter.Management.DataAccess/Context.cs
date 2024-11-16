using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.Core.Miner.Configs;
using MNX.MonitoringCenter.Management.DataAccess.FlightSheet.Dto;
using MNX.MonitoringCenter.Management.DataAccess.FlightSheet.Dto.Target;
using System.Reflection;

namespace MNX.MonitoringCenter.Management.DataAccess;

/// <summary>
/// Контекст БД.
/// </summary>
public class Context : DbContext
{
    internal DbSet<Core.Algorithm> Algorithms { get; set; }

    internal DbSet<Core.Cryptocurrency> Cryptocurrencies { get; set; }

    internal DbSet<FlightSheetDto> FlightSheets { get; set; }

    internal DbSet<BaseFlightSheetTargetDto> FlightSheetTargets { get; set; }

    internal DbSet<MiningCoinConfig> MiningCoinConfigs { get; set; }

    internal DbSet<Core.Miner.Miner> Miners { get; set; }

    internal DbSet<Core.MiningDevice.MiningDeviceInfo> MiningDevices { get; set; }

    internal DbSet<Core.Pool> Pools { get; set; }

    internal DbSet<Core.Preset> Presets { get; set; }

    internal DbSet<Overclocking> Overclocking { get; set; }

    internal DbSet<Core.Wallet> Wallets { get; set; }

    public Context(DbContextOptions<Context> option) : base(option) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}