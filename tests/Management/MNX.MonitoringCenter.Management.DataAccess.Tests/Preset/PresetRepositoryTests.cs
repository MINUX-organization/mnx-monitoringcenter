using AutoMapper;
using MNX.MonitoringCenter.Management.DataAccess.Mapping;
using MNX.MonitoringCenter.Management.DataAccess.Preset;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.Overclockings;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.Overclockings.Fans;
using MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Preset;

using Preset = Core.Overclocking.Preset;

public partial class PresetRepositoryTests : BaseTest
{
    private IPresetRepository _presetRepository;
    private IMapper _mapper;

    [SetUp]
    public void SetUp()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new DbMappingProfile());
        });
        _mapper = config.CreateMapper();

        _presetRepository = new PresetRepository(Context, _mapper);
    }

    private async Task PrepareDataBase(List<Preset> data)
    {
        foreach (var preset in data)
        {
            await _presetRepository.Save(preset);
        }
    }

    private static class PresetsTestCaseSource
    {
        public static Guid UserId = Guid.NewGuid();
        public static string NvidiaGpuName = "Nvidia Geforce RTX 4080 ti";

        public static IEnumerable<List<Preset>> PresetsWithNvidiaGpuOverclocking
        {
            get
            {
                yield return
                [
                    // Невидимый пресет
                    new PresetBuilder()
                        .WithOwnerId(UserId)
                        .WithOverclocking(() => new NvidiaGpuOverclockingBuilder().Build())
                        .Build(),

                    new PresetBuilder()
                        .WithOwnerId(UserId)
                        .WithVisible()
                        .WithOverclocking(() => new NvidiaGpuOverclockingBuilder().Build())
                        .Build(),
                    new PresetBuilder()
                        .WithOwnerId(UserId)
                        .WithVisible()
                        .WithOverclocking(() => new NvidiaGpuOverclockingBuilder().Build())
                        .Build(),
                    new PresetBuilder()
                        .WithOwnerId(UserId)
                        .WithVisible()
                        .WithOverclocking(() => new NvidiaGpuOverclockingBuilder().Build())
                        .Build(),
                    new PresetBuilder()
                        .WithOwnerId(UserId)
                        .WithVisible()
                        .WithOverclocking(() => new NvidiaGpuOverclockingBuilder().Build())
                        .Build(),
                    new PresetBuilder()
                        .WithOwnerId(UserId)
                        .WithVisible()
                        .WithDeviceName(NvidiaGpuName)
                        .WithOverclocking(() => new NvidiaGpuOverclockingBuilder().Build())
                        .Build(),
                    new PresetBuilder()
                        .WithOwnerId(UserId)
                        .WithVisible()
                        .WithDeviceName(NvidiaGpuName)
                        .WithOverclocking(() => new NvidiaGpuOverclockingBuilder().Build())
                        .Build(),
                    new PresetBuilder()
                        .WithOwnerId(UserId)
                        .WithVisible()
                        .WithDeviceName(NvidiaGpuName)
                        .WithOverclocking(() => new NvidiaGpuOverclockingBuilder().Build())
                        .Build(),
                ];
            }
        }

        public static IEnumerable<List<Preset>> PresetsSeparatedByDeviceNameGroups
        {
            get
            {
                yield return
                [
                    new PresetBuilder()
                        .WithOwnerId(UserId)
                        .WithVisible()
                        .WithDeviceName("Nvidia Geforce RTX 4080 ti")
                        .WithOverclocking(() => new NvidiaGpuOverclockingBuilder().Build())
                        .Build(),
                    new PresetBuilder()
                        .WithOwnerId(UserId)
                        .WithVisible()
                        .WithDeviceName("Nvidia Geforce RTX 4080 ti")
                        .WithOverclocking(() => new NvidiaGpuOverclockingBuilder().Build())
                        .Build(),
                    new PresetBuilder()
                        .WithOwnerId(UserId)
                        .WithVisible()
                        .WithDeviceName("Nvidia Geforce RTX 4070")
                        .WithOverclocking(() => new NvidiaGpuOverclockingBuilder().Build())
                        .Build(),
                    new PresetBuilder()
                        .WithOwnerId(UserId)
                        .WithVisible()
                        .WithDeviceName("Nvidia Geforce RTX 4070")
                        .WithOverclocking(() => new NvidiaGpuOverclockingBuilder().Build())
                        .Build(),
                    new PresetBuilder()
                        .WithOwnerId(UserId)
                        .WithVisible()
                        .WithDeviceName("Nvidia GeForce RTX 5080")
                        .WithOverclocking(() => new NvidiaGpuOverclockingBuilder().Build())
                        .Build(),
                    new PresetBuilder()
                        .WithOwnerId(UserId)
                        .WithVisible()
                        .WithDeviceName("Nvidia GeForce RTX 5080")
                        .WithOverclocking(() => new NvidiaGpuOverclockingBuilder().Build())
                        .Build(),
                ];
            }
        }

        public static IEnumerable<Preset> Preset
        {
            get
            {
                yield return new PresetBuilder()
                    .WithOwnerId(UserId)
                    .WithVisible()
                    .WithOverclocking(() =>
                        new NvidiaGpuOverclockingBuilder()
                            .WithFanOverclocking(() =>
                                new FanOverclockingWithTargetSpeedBuilder()
                                    .WithTargetSpeed(75)
                                    .Build())
                            .Build())
                    .Build();
                yield return new PresetBuilder()
                    .WithOwnerId(UserId)
                    .WithVisible()
                    .WithOverclocking(() =>
                        new AmdGpuOverclockingBuilder()
                            .WithFanOverclocking(() =>
                                new FanOverclockingWithTargetTemperatureBuilder()
                                    .WithMaxTargetSpeed(85)
                                    .WithMinTargetSpeed(45)
                                    .WithTargetCoreTemperature(75)
                                    .WithTargetMemoryTemperature(85)
                                    .Build())
                            .Build())
                    .Build();
                yield return new PresetBuilder()
                    .WithOwnerId(UserId)
                    .WithVisible()
                    .WithOverclocking(() =>
                        new AmdGpuOverclockingBuilder()
                            .WithFanOverclocking(() =>
                                new FanOverclockingWithLinearDependenceBuilder()
                                    .AddTargetPoint(point =>
                                        point.WithFanSpeedValueTarget(35)
                                             .WithTemperatureValueTarget(40))
                                    .AddTargetPoint(point =>
                                        point.WithFanSpeedValueTarget(45)
                                             .WithTemperatureValueTarget(50))
                                    .AddTargetPoint(point =>
                                        point.WithFanSpeedValueTarget(55)
                                             .WithTemperatureValueTarget(60))
                                    .AddTargetPoint(point =>
                                        point.WithFanSpeedValueTarget(65)
                                             .WithTemperatureValueTarget(70))
                                    .AddTargetPoint(point =>
                                        point.WithFanSpeedValueTarget(75)
                                             .WithTemperatureValueTarget(80))
                                    .AddTargetPoint(point =>
                                        point.WithFanSpeedValueTarget(85)
                                             .WithTemperatureValueTarget(90))
                                    .AddTargetPoint(point =>
                                        point.WithFanSpeedValueTarget(100)
                                             .WithTemperatureValueTarget(100))
                                    .Build())
                            .Build())
                    .Build();
                yield return new PresetBuilder()
                    .WithOwnerId(UserId)
                    .WithVisible()
                    .WithOverclocking(() =>
                        new IntelGpuOverclockingBuilder().Build())
                    .Build();
            }
        }
    }
}
