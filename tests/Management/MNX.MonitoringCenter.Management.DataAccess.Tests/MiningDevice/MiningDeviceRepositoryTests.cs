using AutoMapper;
using MNX.MonitoringCenter.Management.Core.Mining.FlightSheet.Target;
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Configs;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.Core.Overclocking.Cpu;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu.Fan;
using MNX.MonitoringCenter.Management.DataAccess.FlightSheet;
using MNX.MonitoringCenter.Management.DataAccess.Mapping;
using MNX.MonitoringCenter.Management.DataAccess.MiningDevice;
using MNX.MonitoringCenter.Management.DataAccess.Preset;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.MiningDevices;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.Overclockings;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.Overclockings.Fans;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet;
using MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice;
using MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.MiningDevice;

using Algorithm = Core.Mining.Algorithm;
using Cryptocurrency = Core.Mining.Cryptocurrency;
using FlightSheet = Core.Mining.FlightSheet.FlightSheet;
using Miner = Core.Mining.Miner.Miner;
using MiningDeviceInfo = Core.Mining.MiningDevice.MiningDeviceInfo;
using Pool = Core.Mining.Pool;
using Preset = Core.Overclocking.Preset;
using Wallet = Core.Mining.Wallet;

public partial class MiningDeviceRepositoryTests : BaseTest
{
    private IMiningDeviceRepository _miningDeviceRepository;
    private IPresetRepository _presetRepository;
    private IFlightSheetRepository _flightSheetRepository;
    private IMapper _mapper;

    [SetUp]
    public void SetUp()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<DbMappingProfile>();
        });
        _mapper = config.CreateMapper();
        _miningDeviceRepository = new MiningDeviceRepository(Context, _mapper);
        _presetRepository = new PresetRepository(Context, _mapper);
        _flightSheetRepository = new FlightSheetRepository(Context, _mapper);
    }

    private async Task PrepareDataBase(List<MiningDeviceInfo> data)
    {
        var presets = data
            .Select(x => x.Preset)
            .OfType<Preset>()
            .DistinctBy(x => x.Id);

        var flightSheets = data
            .Select(x => x.FlightSheet)
            .OfType<FlightSheet>()
            .DistinctBy(x => x.Id);

        foreach (var preset in presets)
            await _presetRepository.Save(preset);

        foreach (var flightSheet in flightSheets)
            await _flightSheetRepository.Add(flightSheet);

        await Context.MiningDevices.AddRangeAsync(data);
        await Context.SaveChangesAsync();
    }

    private async Task PrepareDataBase(MiningDeviceInfo data)
    {
        if (data.Preset is not null)
            await _presetRepository.Save(data.Preset);

        if (data.FlightSheet is not null)
            await _flightSheetRepository.Add(data.FlightSheet);

        await Context.MiningDevices.AddAsync(data);
        await Context.SaveChangesAsync();
    }

    private static void AssertDevices(List<MiningDeviceInfo>? expected, List<MiningDeviceInfo>? checking)
    {
        if (expected is null) return;

        Assert.That(checking, Is.Not.Null);
        Assert.That(checking, Has.Count.EqualTo(expected.Count));

        expected = [.. expected.Where(x => x is not null).OrderBy(x => x.Id)];
        checking = [.. checking.Where(x => x is not null).OrderBy(x => x.Id)];

        for (var i = 0; i < expected.Count; i++)
        {
            AssertDevice(expected[i], checking[i]);
        }
    }

    private static void AssertDevice(MiningDeviceInfo expected, MiningDeviceInfo checking)
    {
        Assert.Multiple(() =>
        {
            Assert.That(checking.Id, Is.EqualTo(expected.Id));
            Assert.That(checking.OwnerId, Is.EqualTo(expected.OwnerId));
            Assert.That(checking.Type, Is.EqualTo(expected.Type));
            Assert.That(checking.Manufacturer, Is.EqualTo(expected.Manufacturer));
            Assert.That(checking.Model, Is.EqualTo(expected.Model));
            Assert.That(checking.RigId, Is.EqualTo(expected.RigId));
            Assert.That(checking.LifeCycleStatus, Is.EqualTo(expected.LifeCycleStatus));
            Assert.That(checking.FlightSheetId, Is.EqualTo(expected.FlightSheetId));
            Assert.That(checking.PresetId, Is.EqualTo(expected.PresetId));
            Assert.That(checking.FlightSheetConfirmationState, Is.EqualTo(expected.FlightSheetConfirmationState));
        });
    }

    private static void AssertFlightSheets(List<FlightSheet>? expected, List<FlightSheet>? checking)
    {
        if (expected is null) return;

        Assert.That(checking, Is.Not.Null);
        Assert.That(checking, Has.Count.EqualTo(expected.Count));

        expected = [.. expected.Where(x => x is not null).OrderBy(x => x.Name)];
        checking = [.. checking.Where(x => x is not null).OrderBy(x => x.Name)];

        for (var i = 0; i < expected.Count; i++)
        {
            AssertFlightSheet(expected[i], checking[i]);
        }
    }

    private static void AssertFlightSheet(FlightSheet? expected, FlightSheet? checking)
    {
        if (expected is null) return;

        Assert.That(checking, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(checking.Id, Is.EqualTo(expected.Id));
            Assert.That(checking.OwnerId, Is.EqualTo(expected.OwnerId));
            Assert.That(checking.Name, Is.EqualTo(expected.Name));
            AssertTargets(expected.Targets, checking.Targets);
        });

        static void AssertTargets(List<FlightSheetTarget>? expected, List<FlightSheetTarget>? checking)
        {
            if (expected is null) return;

            expected = [.. expected.OrderBy(x => x.Id)];
            checking = [.. checking.OrderBy(x => x.Id)];

            for (var i = 0; i < expected.Count; i++)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(checking[i].Id, Is.EqualTo(expected[i].Id));
                    Assert.That(checking[i].FlightSheetId, Is.EqualTo(expected[i].FlightSheetId));
                    Assert.That(checking[i].MinerId, Is.EqualTo(expected[i].MinerId));
                    Assert.That(checking[i].DeviceType, Is.EqualTo(expected[i].DeviceType));

                    AssertMiningConfig(expected[i].MiningConfig, checking[i].MiningConfig);
                    AssertMiner(expected[i].Miner, checking[i].Miner);
                });
            }

            void AssertMiningConfig(BaseMiningConfig expected, BaseMiningConfig checking)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(checking.DeviceType, Is.EqualTo(expected?.DeviceType));
                    Assert.That(checking.AdditionalArguments, Is.EqualTo(expected?.AdditionalArguments));
                    Assert.That(checking.ConfigFileContent, Is.EqualTo(expected?.ConfigFileContent));
                    AssertCoinConfigs(expected!.CoinConfigs, checking.CoinConfigs);

                    switch (expected, checking)
                    {
                        case (GpuMiningConfig e, GpuMiningConfig c):
                            AssertGpuMiningConfig(e, c);
                            break;

                        case (CpuMiningConfig e, CpuMiningConfig c):
                            AssertCpuMiningConfig(e, c);
                            break;
                    }
                });

                void AssertCoinConfigs(List<MiningCoinConfig> expected, List<MiningCoinConfig> checking)
                {
                    expected = [.. expected.OrderBy(x => x.Id)];
                    checking = [.. checking.OrderBy(x => x.Id)];

                    for (var i = 0; i < expected.Count; i++)
                    {
                        Assert.Multiple(() =>
                        {
                            Assert.That(checking[i].Id, Is.EqualTo(expected[i].Id));
                            Assert.That(checking[i].PoolId, Is.EqualTo(expected[i].PoolId));
                            Assert.That(checking[i].WalletId, Is.EqualTo(expected[i].WalletId));
                            Assert.That(checking[i].PoolPassword, Is.EqualTo(expected[i].PoolPassword));
                            AssertPool(expected[i].Pool, checking[i].Pool);
                            AssertWallet(expected[i].Wallet, checking[i].Wallet);
                        });
                    }

                    void AssertPool(Pool? expected, Pool? checking)
                    {
                        Assert.Multiple(() =>
                        {
                            Assert.That(checking?.Id, Is.EqualTo(expected?.Id));
                            Assert.That(checking?.CryptocurrencyId, Is.EqualTo(expected?.CryptocurrencyId));
                            Assert.That(checking?.OwnerId, Is.EqualTo(expected?.OwnerId));
                            Assert.That(checking?.Domain, Is.EqualTo(expected?.Domain));
                            Assert.That(checking?.Port, Is.EqualTo(expected?.Port));
                            Assert.That(checking?.Tls, Is.EqualTo(expected?.Tls));
                            AssertCryptocurrency(expected?.Cryptocurrency, checking?.Cryptocurrency);
                        });
                    }

                    void AssertWallet(Wallet? expected, Wallet? checking)
                    {
                        Assert.Multiple(() =>
                        {
                            Assert.That(checking?.Id, Is.EqualTo(expected?.Id));
                            Assert.That(checking?.CryptocurrencyId, Is.EqualTo(expected?.CryptocurrencyId));
                            Assert.That(checking?.OwnerId, Is.EqualTo(expected?.OwnerId));
                            Assert.That(checking?.Name, Is.EqualTo(expected?.Name));
                            Assert.That(checking?.Address, Is.EqualTo(expected?.Address));
                            AssertCryptocurrency(expected?.Cryptocurrency, checking?.Cryptocurrency);
                        });
                    }

                    void AssertCryptocurrency(Cryptocurrency? expected, Cryptocurrency? checking)
                    {
                        Assert.Multiple(() =>
                        {
                            Assert.That(checking?.Id, Is.EqualTo(expected?.Id));
                            Assert.That(checking?.OwnerId, Is.EqualTo(expected?.OwnerId));
                            Assert.That(checking?.AlgorithmId, Is.EqualTo(expected?.AlgorithmId));
                            Assert.That(checking?.FullName, Is.EqualTo(expected?.FullName));
                            Assert.That(checking?.ShortName, Is.EqualTo(expected?.ShortName));
                            AssertAlgorithm(expected?.Algorithm, checking?.Algorithm);
                        });

                        void AssertAlgorithm(Algorithm? expected, Algorithm? checking)
                        {
                            Assert.Multiple(() =>
                            {
                                Assert.That(checking?.Id, Is.EqualTo(expected?.Id));
                                Assert.That(checking?.OwnerId, Is.EqualTo(expected?.OwnerId));
                                Assert.That(checking?.Name, Is.EqualTo(expected?.Name));
                            });
                        }
                    }
                }

                void AssertCpuMiningConfig(CpuMiningConfig expected, CpuMiningConfig checking)
                {
                    Assert.Multiple(() =>
                    {
                        Assert.That(checking.ThreadsCount, Is.EqualTo(expected?.ThreadsCount));
                        Assert.That(checking.HugePages, Is.EqualTo(expected?.HugePages));
                    });
                }

                void AssertGpuMiningConfig(GpuMiningConfig expected, GpuMiningConfig checking)
                {
                    // Дополнительных полей нет.
                }
            }

            void AssertMiner(Miner? expected, Miner? checking)
            {
                if (expected is null) return;

                Assert.That(checking, Is.Not.Null);
                Assert.Multiple(() =>
                {
                    Assert.That(checking.Id, Is.EqualTo(expected.Id));
                    Assert.That(checking.Name, Is.EqualTo(expected.Name));
                    Assert.That(checking.Version, Is.EqualTo(expected.Version));
                    Assert.That(checking.InstallationUrl, Is.EqualTo(expected.InstallationUrl));
                    Assert.That(checking.Type, Is.EqualTo(expected.Type));
                    Assert.That(checking.SupportedDevices, Is.EqualTo(expected.SupportedDevices));
                    Assert.That(checking.MiningMode, Is.EqualTo(expected.MiningMode));
                    Assert.That(checking.OwnerId, Is.EqualTo(expected.OwnerId));
                    Assert.That(checking.PoolTemplate, Is.EqualTo(expected.PoolTemplate));
                    Assert.That(checking.WalletWorkerTemplate, Is.EqualTo(expected.WalletWorkerTemplate));
                });
            }
        }
    }

    private static void AssertPresets(List<Preset>? expected, List<Preset>? checking)
    {
        if (expected is null) return;


        Assert.That(checking, Is.Not.Null);
        Assert.That(checking, Has.Count.EqualTo(expected.Count));

        expected = [.. expected.Where(x => x is not null).OrderBy(x => x.Name)];
        checking = [.. checking.Where(x => x is not null).OrderBy(x => x.Name)];

        for (var i = 0; i < expected.Count; i++)
        {
            AssertPreset(expected[i], checking[i]);
        }
    }

    private static void AssertPreset(Preset? expected, Preset? checking)
    {
        if (expected is null) return;

        Assert.That(checking, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(checking.Id, Is.EqualTo(expected.Id));
            Assert.That(checking.Name, Is.EqualTo(expected.Name));
            Assert.That(checking.DeviceName, Is.EqualTo(expected.DeviceName));
            Assert.That(checking.IsVisible, Is.EqualTo(expected.IsVisible));
            Assert.That(checking.OwnerId, Is.EqualTo(expected.OwnerId));
            Assert.That(checking.OverclockingId, Is.EqualTo(expected.OverclockingId));
        });
    }

    private static void AssertOverclocking(IOverclocking? expected, IOverclocking? checking)
    {
        if (expected is null) return;

        Assert.That(checking, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(checking.Id, Is.EqualTo(expected.Id));
            Assert.That(checking.TargetDeviceType, Is.EqualTo(expected.TargetDeviceType));
            switch (expected, checking)
            {
                case (NvidiaGpuOverclocking e, NvidiaGpuOverclocking c):
                    AssertNvidiaGpuOverclocking(e, c);
                    break;
                case (AmdGpuOverclocking e, AmdGpuOverclocking c):
                    AssertAmdGpuOverclocking(e, c);
                    break;
                case (IntelGpuOverclocking e, IntelGpuOverclocking c):
                    AssertIntelGpuOverclocking(e, c);
                    break;
                case (CpuOverclocking e, CpuOverclocking c):
                    AssertCpuOverclocking(e, c);
                    break;
            }
        });

        static void AssertNvidiaGpuOverclocking(NvidiaGpuOverclocking expected, NvidiaGpuOverclocking checking)
        {
            Assert.Multiple(() =>
            {
                Assert.That(checking.CoreClockLock, Is.EqualTo(expected.CoreClockLock));
                Assert.That(checking.CoreClockOffset, Is.EqualTo(expected.CoreClockOffset));
                Assert.That(checking.CoreVoltage, Is.EqualTo(expected.CoreVoltage));
                Assert.That(checking.CoreVoltageOffset, Is.EqualTo(expected.CoreVoltageOffset));
                Assert.That(checking.MemoryClockLock, Is.EqualTo(expected.MemoryClockLock));
                Assert.That(checking.MemoryClockOffset, Is.EqualTo(expected.MemoryClockOffset));
                Assert.That(checking.MemoryVoltage, Is.EqualTo(expected.MemoryVoltage));
                Assert.That(checking.MemoryVoltageOffset, Is.EqualTo(expected.MemoryVoltageOffset));
                Assert.That(checking.PowerLimit, Is.EqualTo(expected.PowerLimit));
                AssertFanOverclocking(expected.FanOverclocking, checking.FanOverclocking);
            });
        }

        static void AssertAmdGpuOverclocking(AmdGpuOverclocking expected, AmdGpuOverclocking checking)
        {
            Assert.Multiple(() =>
            {
                Assert.That(checking.CoreClockLock, Is.EqualTo(expected.CoreClockLock));
                Assert.That(checking.CoreVoltage, Is.EqualTo(expected.CoreVoltage));
            });
        }

        static void AssertIntelGpuOverclocking(IntelGpuOverclocking expected, IntelGpuOverclocking checking)
        {
            // Нет дополнительных свойств.
        }

        static void AssertCpuOverclocking(CpuOverclocking expected, CpuOverclocking checking)
        {
            Assert.Multiple(() =>
            {
                Assert.That(checking.CoreClockLock, Is.EqualTo(expected.CoreClockLock));
                Assert.That(checking.CoreVoltage, Is.EqualTo(expected.CoreVoltage));
            });
        }

        static void AssertFanOverclocking(IFanOverclocking? expected, IFanOverclocking? checking)
        {
            if (expected is null) return;

            Assert.That(checking, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(checking.Id, Is.EqualTo(expected.Id));
                Assert.That(checking.Type, Is.EqualTo(expected.Type));
                switch (expected, checking)
                {
                    case (FanOverclockingWithTargetSpeed e, FanOverclockingWithTargetSpeed c):
                        AssertTargetSpeed(e, c);
                        break;
                    case (FanOverclockingWithTargetTemperature e, FanOverclockingWithTargetTemperature c):
                        AssertTargetTemperature(e, c);
                        break;
                    case (FanOverclockingWithLinearDependence e, FanOverclockingWithLinearDependence c):
                        AssertLinearDependence(e, c);
                        break;
                }
            });

            static void AssertTargetSpeed(FanOverclockingWithTargetSpeed expected, FanOverclockingWithTargetSpeed checking)
            {
                Assert.That(checking.TargetSpeed, Is.EqualTo(expected.TargetSpeed));
            }

            static void AssertTargetTemperature(FanOverclockingWithTargetTemperature expected, FanOverclockingWithTargetTemperature checking)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(checking.MaxTargetSpeed, Is.EqualTo(expected.MaxTargetSpeed));
                    Assert.That(checking.MinTargetSpeed, Is.EqualTo(expected.MinTargetSpeed));
                    Assert.That(checking.TargetCoreTemperature, Is.EqualTo(expected.TargetCoreTemperature));
                    Assert.That(checking.TargetMemoryTemperature, Is.EqualTo(expected.TargetMemoryTemperature));
                });
            }

            static void AssertLinearDependence(FanOverclockingWithLinearDependence expected, FanOverclockingWithLinearDependence checking)
            {
                Assert.That(checking.TargetPoints, Has.Length.EqualTo(expected.TargetPoints.Length));
                for (var i = 0; i < expected.TargetPoints.Length; i++)
                {
                    Assert.Multiple(() =>
                    {
                        Assert.That(checking.TargetPoints[i].PointIndex, Is.EqualTo(expected.TargetPoints[i].PointIndex));
                        Assert.That(checking.TargetPoints[i].TemperatureValueTarget, Is.EqualTo(expected.TargetPoints[i].TemperatureValueTarget));
                        Assert.That(checking.TargetPoints[i].FanSpeedValueTarget, Is.EqualTo(expected.TargetPoints[i].FanSpeedValueTarget));
                    });
                }
            }
        }
    }

    private static class MiningDevicesTestCaseSource
    {
        public static Guid UserId = Guid.NewGuid();
        public static Guid RigId = Guid.NewGuid();
        public static Guid PresetId = Guid.NewGuid();

        public static IEnumerable<List<MiningDeviceInfo>> MiningDeviceLists
        {
            get
            {
                yield return
                [
                    new MiningDeviceInfoBuilder()
                        .WithOwner(UserId)
                        .WithDeviceType(MiningDeviceType.GPU)
                        .WithRig(RigId)
                        .WithLifeCycleStatus(MiningDeviceLifeCycleStatus.Online)
                        .WithFlightSheet()
                        .WithConfirmationState(FlightSheetConfirmationState.Unconfirmed)
                        .WithPreset(preset =>
                            preset.WithOwnerId(UserId)
                                  .WithVisible()
                                  .WithOverclocking(() => new NvidiaGpuOverclockingBuilder().Build()))
                        .Build(),
                    new MiningDeviceInfoBuilder()
                        .WithOwner(UserId)
                        .WithDeviceType(MiningDeviceType.GPU)
                        .WithRig(RigId)
                        .WithLifeCycleStatus(MiningDeviceLifeCycleStatus.Online)
                        .WithFlightSheet()
                        .WithConfirmationState(FlightSheetConfirmationState.Unconfirmed)
                        .WithPreset(preset =>
                            preset.WithOwnerId(UserId)
                                  .WithVisible()
                                  .WithOverclocking(() => new NvidiaGpuOverclockingBuilder().Build()))
                        .Build(),
                    new MiningDeviceInfoBuilder()
                        .WithOwner(UserId)
                        .WithDeviceType(MiningDeviceType.GPU)
                        .WithRig(RigId)
                        .WithLifeCycleStatus(MiningDeviceLifeCycleStatus.Online)
                        .WithFlightSheet()
                        .WithConfirmationState(FlightSheetConfirmationState.Unconfirmed)
                        .WithPreset(preset =>
                            preset.WithOwnerId(UserId)
                                  .WithVisible()
                                  .WithOverclocking(() => new NvidiaGpuOverclockingBuilder().Build()))
                        .Build(),
                    new MiningDeviceInfoBuilder()
                        .WithOwner(UserId)
                        .WithDeviceType(MiningDeviceType.CPU)
                        .WithFlightSheet()
                        .WithConfirmationState(FlightSheetConfirmationState.Unconfirmed)
                        .WithPreset(preset =>
                            preset.WithOwnerId(UserId)
                                  .WithVisible()
                                  .WithOverclocking(() => new CpuOverclockingBuilder().Build()))
                        .Build(),
                ];
            }
        }

        public static IEnumerable<List<MiningDeviceInfo>> GpuDevicesWithUnionPreset
        {
            get
            {
                var preset = new PresetBuilder()
                    .WithId(PresetId)
                    .WithVisible()
                    .WithOwnerId(UserId)
                    .WithOverclocking(() => new AmdGpuOverclockingBuilder().Build())
                    .Build();

                yield return
                [
                    new MiningDeviceInfoBuilder()
                        .WithOwner(UserId)
                        .WithDeviceType(MiningDeviceType.GPU)
                        .WithLifeCycleStatus(MiningDeviceLifeCycleStatus.Online)
                        .WithConfirmationState(FlightSheetConfirmationState.Successfully)
                        .WithRig(RigId)
                        .WithPreset(preset)
                        .Build(),
                    new MiningDeviceInfoBuilder()
                        .WithOwner(UserId)
                        .WithDeviceType(MiningDeviceType.GPU)
                        .WithLifeCycleStatus(MiningDeviceLifeCycleStatus.Online)
                        .WithConfirmationState(FlightSheetConfirmationState.Successfully)
                        .WithRig(RigId)
                        .WithPreset(preset)
                        .Build(),
                    new MiningDeviceInfoBuilder()
                        .WithOwner(UserId)
                        .WithDeviceType(MiningDeviceType.GPU)
                        .WithLifeCycleStatus(MiningDeviceLifeCycleStatus.Online)
                        .WithConfirmationState(FlightSheetConfirmationState.Successfully)
                        .WithRig(RigId)
                        .WithPreset(preset)
                        .Build(),
                ];
            }
        }

        public static IEnumerable<MiningDeviceInfo> MiningDevices
        {
            get
            {
                yield return new MiningDeviceInfoBuilder()
                    .WithOwner(UserId)
                    .WithDeviceType(MiningDeviceType.GPU)
                    .WithRig(RigId)
                    .WithLifeCycleStatus(MiningDeviceLifeCycleStatus.Online)
                    .Build();
                yield return new MiningDeviceInfoBuilder()
                    .WithOwner(UserId)
                    .WithDeviceType(MiningDeviceType.GPU)
                    .WithRig(RigId)
                    .WithLifeCycleStatus(MiningDeviceLifeCycleStatus.Online)
                    .Build();
                yield return new MiningDeviceInfoBuilder()
                    .WithOwner(UserId)
                    .WithDeviceType(MiningDeviceType.GPU)
                    .WithRig(RigId)
                    .WithLifeCycleStatus(MiningDeviceLifeCycleStatus.Offline)
                    .Build();
            }
        }

        public static IEnumerable<MiningDeviceInfo> InactiveMiningDevices
        {
            get
            {
                yield return new MiningDeviceInfoBuilder()
                    .WithOwner(UserId)
                    .WithDeviceType(MiningDeviceType.GPU)
                    .WithRig(RigId)
                    .WithLifeCycleStatus(MiningDeviceLifeCycleStatus.Inactive)
                    .Build();
                yield return new MiningDeviceInfoBuilder()
                    .WithOwner(UserId)
                    .WithDeviceType(MiningDeviceType.GPU)
                    .WithRig(RigId)
                    .WithLifeCycleStatus(MiningDeviceLifeCycleStatus.Inactive)
                    .Build();
                yield return new MiningDeviceInfoBuilder()
                    .WithOwner(UserId)
                    .WithDeviceType(MiningDeviceType.GPU)
                    .WithRig(RigId)
                    .WithLifeCycleStatus(MiningDeviceLifeCycleStatus.Inactive)
                    .Build();
            }
        }

        public static IEnumerable<MiningDeviceInfo> MiningDevicesWithOnlineStatus
        {
            get
            {
                yield return new MiningDeviceInfoBuilder()
                    .WithOwner(UserId)
                    .WithDeviceType(MiningDeviceType.GPU)
                    .WithRig(RigId)
                    .WithLifeCycleStatus(MiningDeviceLifeCycleStatus.Online)
                    .Build();
                yield return new MiningDeviceInfoBuilder()
                    .WithOwner(UserId)
                    .WithDeviceType(MiningDeviceType.GPU)
                    .WithRig(RigId)
                    .WithLifeCycleStatus(MiningDeviceLifeCycleStatus.Online)
                    .Build();
                yield return new MiningDeviceInfoBuilder()
                    .WithOwner(UserId)
                    .WithDeviceType(MiningDeviceType.GPU)
                    .WithRig(RigId)
                    .WithLifeCycleStatus(MiningDeviceLifeCycleStatus.Online)
                    .Build();
            }
        }

        public static IEnumerable<MiningDeviceInfo> MiningDevicesWithOverclockings
        {
            get
            {
                yield return new MiningDeviceInfoBuilder()
                    .WithOwner(UserId)
                    .WithDeviceType(MiningDeviceType.GPU)
                    .WithPreset(preset =>
                        preset.WithVisible()
                              .WithOverclocking(() =>
                            new NvidiaGpuOverclockingBuilder()
                                .WithCoreClockLock(100)
                                .WithCoreClockOffset(200)
                                .WithCoreVoltage(250)
                                .WithCoreVoltageOffset(-50)
                                .WithMemoryClockLock(2500)
                                .WithMemoryClockOffset(45)
                                .WithMemoryVoltage(1200)
                                .WithMemoryVoltageOffset(-100)
                                .WithPowerLimit(45)
                                .WithFanOverclocking(() =>
                                    new FanOverclockingWithTargetSpeedBuilder()
                                        .WithTargetSpeed(45)
                                        .Build())
                                .Build()))
                    .WithRig(RigId)
                    .Build();
                yield return new MiningDeviceInfoBuilder()
                    .WithOwner(UserId)
                    .WithDeviceType(MiningDeviceType.GPU)
                    .WithRig(RigId)
                    .WithPreset(preset =>
                        preset.WithVisible()
                              .WithOverclocking(() =>
                            new AmdGpuOverclockingBuilder()
                                .WithCoreClockLock(150)
                                .WithCoreClockState(75)
                                .WithCoreVoltage(205)
                                .WithCoreVoltageOffset(-60)
                                .WithMemoryClockLock(240)
                                .WithMemoryClockState(420)
                                .WithMemoryVoltage(220)
                                .WithMemoryControllerVoltage(140)
                                .WithMemoryTweak("tweak")
                                .WithAlternativeDownVoltage()
                                .WithEnhancedOverclock()
                                .WithPowerLimit(200)
                                .WithSocFrequency(120)
                                .WithSocVoltage(150)
                                .WithFanOverclocking(() =>
                                    new FanOverclockingWithTargetTemperatureBuilder()
                                        .WithMaxTargetSpeed(75)
                                        .WithMinTargetSpeed(40)
                                        .WithTargetCoreTemperature(85)
                                        .WithTargetMemoryTemperature(70)
                                        .Build())
                                .Build()))
                    .Build();
                yield return new MiningDeviceInfoBuilder()
                    .WithOwner(UserId)
                    .WithDeviceType(MiningDeviceType.CPU)
                    .WithRig(RigId)
                    .WithPreset(preset =>
                        preset.WithOverclocking(() =>
                            new CpuOverclockingBuilder()
                                .WithCoreClockLock(240)
                                .WithCoreVoltage(120)
                                .Build()))
                    .Build();
            }
        }
    }
}
