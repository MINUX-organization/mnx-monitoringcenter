using AutoMapper;
using MNX.MonitoringCenter.Management.DataAccess.Mapping;
using MNX.MonitoringCenter.Management.DataAccess.Preset;
using MNX.MonitoringCenter.Management.DataAccess.Tests.Infrastructure;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.Overclockings;
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
    }
}
