using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Monitoring.Core;
using MNX.MonitoringCenter.Monitoring.DataAccess.Dto;
using MNX.MonitoringCenter.Monitoring.DataAccess.Dto.Devices;
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

    /// <summary>
    /// Майнинг устройства.
    /// </summary>
    public DbSet<MiningDeviceDto> MiningDevices { get; set; }

    /// <summary>
    /// Разгон майнинг устройств.
    /// </summary>
    public DbSet<OverclockingDto> Overclocking { get; set; }

    public Context(DbContextOptions<Context> option) : base(option) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<Coin>().HasData(
            new Coin()
            {
                Id = Guid.Parse("74e7b0c4-9a96-4359-b5dc-66e20988f26a"),
                FullName = "Bitcoin",
                ShortName = "BTC"
            },
            new Coin()
            {
                Id = Guid.Parse("74e7b0c4-9a96-4359-b5dc-66e20988f26b"),
                FullName = "Ethereum",
                ShortName = "ETH"
            });

        modelBuilder.Entity<OverclockingDto>().HasData(
            new OverclockingDto()
            {
                Id = Guid.Parse("4f0aab97-2c80-4a47-9c1b-11aabdb6da01"),
                CoreClockLock = 0,
                CoreClockOffset = 0,
                MemoryClockLock = 0,
                MemoryClockOffset = 0,
                CoreVoltage = 0,
                CoreVoltageOffset = 0,
                MemoryVoltage = 0,
                MemoryVoltageOffset = 0,
                PowerLimit = 0,
                CriticalTemperature = 0,
                FanSpeed = 0
            });

        modelBuilder.Entity<FlightSheetCoinDto>().HasData(
            new FlightSheetCoinDto()
            {
                CoinId = Guid.Parse("74e7b0c4-9a96-4359-b5dc-66e20988f26a"),
                FlightSheetId = Guid.Parse("0599c30c-75cc-404c-b150-5d5205f5cffa")
            },
            new FlightSheetCoinDto()
            {
                CoinId = Guid.Parse("74e7b0c4-9a96-4359-b5dc-66e20988f26b"),
                FlightSheetId = Guid.Parse("0599c30c-75cc-404c-b150-5d5205f5cffa")
            });

        modelBuilder.Entity<FlightSheetDto>().HasData(new FlightSheetDto()
        {
            Id = Guid.Parse("0599c30c-75cc-404c-b150-5d5205f5cffa"),
            Name = "flight_sheet_1"
        });

        modelBuilder.Entity<CpuDto>().HasBaseType<MiningDeviceDto>().HasData(new CpuDto()
        {
            Id = Guid.NewGuid(),
            RigId = Guid.Parse("6a0a78d9-dcb5-4b5b-b6d0-17e321ab42b8"),
            UserId = 1,
            Name = "cpu_1",
            Type = Core.Devices.Enums.MiningDeviceType.CPU,
            Manufacturer = Core.Devices.Enums.CpuManufacturerEnum.Intel.ToString(),
            FlightSheetId = Guid.Parse("0599c30c-75cc-404c-b150-5d5205f5cffa"),
            Miner = "miner_1",
            SerialNumber = "1.0.0",
            Architecture = "x64",
            CoresCount = 6,
            ThreadsCount = 12,
            ThreadsPerSocketCount = 6,
            MinClock = 1F,
            MaxClock = 3.4F
        });

        modelBuilder.Entity<GpuDto>().HasBaseType<MiningDeviceDto>().HasData(new GpuDto()
        {
            Id = Guid.Parse("0599c30c-dcb5-4b5b-b6d0-17e321ab42b8"),
            RigId = Guid.Parse("6a0a78d9-dcb5-4b5b-b6d0-17e321ab42b8"),
            UserId = 1,
            Name = "gpu_1",
            Type = Core.Devices.Enums.MiningDeviceType.GPU,
            Manufacturer = Core.Devices.Enums.GpuManufacturerEnum.Amd.ToString(),
            FlightSheetId = Guid.Parse("0599c30c-75cc-404c-b150-5d5205f5cffa"),
            Miner = "miner_2",
            SerialNumber = "1.0.0",
            PciBusId = 4,
            CriticalTemperature = 100,
            PowerLimit = 100,
            Technology = Core.Devices.Enums.ParallelComputingTechnologyEnum.CUDA,
            TechnologyVersion = "1.0.0",
            Vendor = "vendor",
            MemorySize = 100,
            MemoryType = "memory_type",
            MemoryVendor = "memory_vendor",
            BiosVersion = "1.0.0",
            OverclockingId = Guid.Parse("4f0aab97-2c80-4a47-9c1b-11aabdb6da01")
        });

        modelBuilder.Entity<RigDto>().HasData(new RigDto()
        {
            Id = Guid.Parse("6a0a78d9-dcb5-4b5b-b6d0-17e321ab42b8"),
            UserId = 1,
            Name = "rig_1",
            GlobalIP = "127.0.0.1",
            LocalIP = "127.0.0.1",
            Mac = "40-8D-3D--64-72-HD",
            MinuxVersion = "1.0.0",
            LinuxVersion = "1.0.0",
            AmdDriverVersion = "1.0.0",
            NvidiaDriverVersion = "1.0.0",
            IntelDriverVersion = "1.0.0",
            OpenCLVersion = "1.0.0",
            CudaVersion = "1.0.0"
        });
    }
}