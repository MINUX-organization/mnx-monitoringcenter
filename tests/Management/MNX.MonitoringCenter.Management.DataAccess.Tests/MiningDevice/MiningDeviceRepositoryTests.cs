using AutoMapper;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;
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

using FlightSheet = Core.Mining.FlightSheet.FlightSheet;
using MiningDeviceInfo = Core.Mining.MiningDevice.MiningDeviceInfo;
using Preset = Core.Overclocking.Preset;

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
            .DistinctBy(x => x.Id)
            .ToList();

        var flightSheets = data
            .Select(x => x.FlightSheet)
            .OfType<FlightSheet>()
            .DistinctBy(x => x.Id)
            .ToList();

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

    private static class MiningDevicesTestCaseSource
    {
        public static Guid UserId = Guid.NewGuid();
        public static Guid RigId = Guid.NewGuid();
        public static Guid PresetId = Guid.NewGuid();

        public static IEnumerable<List<MiningDeviceInfo>> MiningDeviceListsWithVisiblePreset
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

        public static IEnumerable<List<MiningDeviceInfo>> GpuDevicesWithUnionVisiblePreset
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
