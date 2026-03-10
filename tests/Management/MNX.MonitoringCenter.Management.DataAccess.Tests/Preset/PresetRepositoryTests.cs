using AutoMapper;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu.Fan;
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

    private static void AssertPresetsWithNvidiaGpuOverclocking(List<Preset> expected, List<Preset> checking)
    {
        expected = expected.OrderBy(x => x.Name).ToList();
        checking = checking.OrderBy(x => x.Name).ToList();

        for (var i = 0; i < expected.Count; i++)
        {
            Assert.Multiple(() =>
            {
                Assert.That(checking[i].Id, Is.EqualTo(expected[i].Id));
                Assert.That(checking[i].Name, Is.EqualTo(expected[i].Name));
                Assert.That(checking[i].DeviceName, Is.EqualTo(expected[i].DeviceName));
                Assert.That(checking[i].IsVisible, Is.EqualTo(expected[i].IsVisible));
                Assert.That(checking[i].OwnerId, Is.EqualTo(expected[i].OwnerId));
                Assert.That(checking[i].OverclockingId, Is.EqualTo(expected[i].OverclockingId));

                Assert.That(checking[i].Overclocking, Is.Not.Null);
                Assert.That(checking[i].Overclocking, Is.TypeOf<NvidiaGpuOverclocking>());
                var checkingOverclocking = (NvidiaGpuOverclocking)checking[i].Overclocking!;
                var expectedOverclocking = (NvidiaGpuOverclocking)expected[i].Overclocking!;
                AssertOverclockings(expectedOverclocking, checkingOverclocking);
            });
        }

        void AssertOverclockings(NvidiaGpuOverclocking expected, NvidiaGpuOverclocking checking)
        {
            Assert.Multiple(() =>
            {
                Assert.That(checking.Id, Is.EqualTo(expected.Id));
                Assert.That(checking.TargetDeviceType, Is.EqualTo(expected.TargetDeviceType));
                Assert.That(checking.PowerLimit, Is.EqualTo(expected.PowerLimit));
                Assert.That(checking.CoreClockLock, Is.EqualTo(expected.CoreClockLock));
                Assert.That(checking.CoreClockOffset, Is.EqualTo(expected.CoreClockOffset));
                Assert.That(checking.CoreVoltage, Is.EqualTo(expected.CoreVoltage));
                Assert.That(checking.CoreVoltageOffset, Is.EqualTo(expected.CoreVoltageOffset));
                Assert.That(checking.MemoryClockLock, Is.EqualTo(expected.MemoryClockLock));
                Assert.That(checking.MemoryClockOffset, Is.EqualTo(expected.MemoryClockOffset));
                Assert.That(checking.MemoryVoltage, Is.EqualTo(expected.MemoryVoltage));
                Assert.That(checking.MemoryVoltageOffset, Is.EqualTo(expected.MemoryVoltageOffset));

                Assert.That(checking.FanOverclocking, Is.Not.Null);
                Assert.That(checking.FanOverclocking, Is.TypeOf<FanOverclockingWithTargetSpeed>());
                var expectedFanOverclocking = (FanOverclockingWithTargetSpeed)expected.FanOverclocking;
                var checkingFanOverclocking = (FanOverclockingWithTargetSpeed)checking.FanOverclocking;
                AssertFanOverclockings(expectedFanOverclocking, checkingFanOverclocking);
            });

            void AssertFanOverclockings(FanOverclockingWithTargetSpeed expected, FanOverclockingWithTargetSpeed checking)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(checking.Id, Is.EqualTo(expected.Id));
                    Assert.That(checking.Type, Is.EqualTo(expected.Type));
                    Assert.That(checking.TargetSpeed, Is.EqualTo(expected.TargetSpeed));
                });
            }
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
            // CPU-разгон на данный момент не поддерживается.
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
                                        point.WithPointIndex(0)
                                             .WithFanSpeedValueTarget(35)
                                             .WithTemperatureValueTarget(40))
                                    .AddTargetPoint(point =>
                                        point.WithPointIndex(0)
                                             .WithFanSpeedValueTarget(45)
                                             .WithTemperatureValueTarget(50))
                                    .AddTargetPoint(point =>
                                        point.WithPointIndex(0)
                                             .WithFanSpeedValueTarget(55)
                                             .WithTemperatureValueTarget(60))
                                    .AddTargetPoint(point =>
                                        point.WithPointIndex(0)
                                             .WithFanSpeedValueTarget(65)
                                             .WithTemperatureValueTarget(70))
                                    .AddTargetPoint(point =>
                                        point.WithPointIndex(0)
                                             .WithFanSpeedValueTarget(75)
                                             .WithTemperatureValueTarget(80))
                                    .AddTargetPoint(point =>
                                        point.WithPointIndex(0)
                                             .WithFanSpeedValueTarget(85)
                                             .WithTemperatureValueTarget(90))
                                    .AddTargetPoint(point =>
                                        point.WithPointIndex(0)
                                             .WithFanSpeedValueTarget(100)
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
