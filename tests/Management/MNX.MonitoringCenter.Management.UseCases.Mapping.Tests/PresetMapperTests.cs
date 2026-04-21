using MNX.MonitoringCenter.Management.Contracts.Overclocking.Cpu;
using MNX.MonitoringCenter.Management.Contracts.Presets;
using MNX.MonitoringCenter.Management.Core.Overclocking.Cpu;
using MNX.MonitoringCenter.Management.Tests.Service.Assertions;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders.Overclockings;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.Overclockings;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Infrastructure.Overclocking.Model;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Preset;
using MNX.MonitoringCenter.Management.UseCases.Overclocking;
using MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets;
using MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets.Commands;
using Moq;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests;

using Preset = Core.Overclocking.Preset;

[TestFixture]
public sealed class PresetMapperTests
{
    private Mock<IOverclockingModelMapper<CpuOverclockingModel, CpuOverclocking>> _cpuOverclockingMapper;
    private OverclockingModelMapperRegistry _overclockingMapperRegistry;
    private IPresetMapper _presetMapper;

    [SetUp]
    public void SetUp()
    {
        _cpuOverclockingMapper = new Mock<IOverclockingModelMapper<CpuOverclockingModel, CpuOverclocking>>();
        _overclockingMapperRegistry = new OverclockingModelMapperRegistry();
        _overclockingMapperRegistry.Register(_cpuOverclockingMapper.Object);
        _presetMapper = new PresetMapper(_overclockingMapperRegistry);
    }

    [Test]
    public void MapToCoreEntity_ValidInputModelAndOwnerId_ReturnsCoreEntity()
    {
        // Arrange

        var ownerId = Guid.NewGuid();
        var presetModel = CreatePresetInputModel();

        _cpuOverclockingMapper.Setup(x => x.MapToCoreEntity(It.IsAny<CpuOverclockingModel>()))
            .Returns(CreateOverclocking());

        // Act

        var mappedPreset = _presetMapper.MapToCoreEntity(presetModel, ownerId);


        // Assert

        mappedPreset.ShouldBeEqualTo(presetModel, ownerId);

        _cpuOverclockingMapper.Verify(x => x.MapToCoreEntity(It.IsAny<CpuOverclockingModel>()), Times.Once);
    }

    [Test]
    public void MapToCoreEntity_ValidInputModelAndOriginalCoreEntity_ReturnsCoreEntity()
    {
        // Arrange

        var presetModel = CreatePresetInputModel();
        var originalPreset = CreatePreset();

        _cpuOverclockingMapper.Setup(x =>
            x.MapToCoreEntity(It.IsAny<CpuOverclockingModel>(),
                              It.IsAny<CpuOverclocking>()))
            .Returns(CreateOverclocking());


        // Act

        var mappedPreset = _presetMapper.MapToCoreEntity(presetModel, originalPreset);


        // Assert

        mappedPreset.ShouldBeEqualTo(presetModel, originalPreset);

        _cpuOverclockingMapper.Verify(x => x.MapToCoreEntity(
            It.IsAny<CpuOverclockingModel>(),
            It.IsAny<CpuOverclocking>()), Times.Once);
    }

    [TestCaseSource(typeof(PresetTestsCaseSources), nameof(PresetTestsCaseSources.CorePresetLists))]
    public void MapToCoreEntitiesList_ValidCoreEntitiesList_ReturnsModelsList(List<Preset> data)
    {
        // Arrange

        _cpuOverclockingMapper.Setup(x => x.MapToModel(It.IsAny<CpuOverclocking>()))
            .Returns(CreateOverclockingModel());


        // Act

        var mappedPresetModelList = _presetMapper.MapToCoreEntitiesList(data);


        // Assert

        mappedPresetModelList.ShouldBeEqualTo(data);

        _cpuOverclockingMapper.Verify(x => x.MapToModel(It.IsAny<CpuOverclocking>()), Times.AtLeast(data.Count));
    }

    [Test]
    public void MapToModel_ValidCoreEntity_ReturnsModel()
    {
        // Arrange

        var preset = CreatePreset();

        _cpuOverclockingMapper.Setup(x => x.MapToModel(It.IsAny<CpuOverclocking>()))
            .Returns(CreateOverclockingModel());


        // Act

        var mappedPresetModel = _presetMapper.MapToModel(preset);


        // Assert

        Assert.That(mappedPresetModel, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(mappedPresetModel.Id, Is.EqualTo(preset.Id));
            Assert.That(mappedPresetModel.Name, Is.EqualTo(preset.Name));
            Assert.That(mappedPresetModel.DeviceName, Is.EqualTo(preset.DeviceName));
            Assert.That(mappedPresetModel.Overclocking, Is.Not.Null);
        });

        _cpuOverclockingMapper.Verify(x => x.MapToModel(It.IsAny<CpuOverclocking>()), Times.Once);
    }

    private Preset CreatePreset()
    {
        return new PresetBuilder()
            .WithDeviceName("Nvidia Geforce RTX 4060 ti")
            .WithOverclocking(CreateOverclocking)
            .Build();
    }

    private PresetInputModel CreatePresetInputModel()
    {
        return new PresetInputModelBuilder()
            .WithDeviceName("Nvidia Geforce RTX 4060 ti")
            .WithOverclocking(CreateOverclockingModel)
            .Build();
    }

    private CpuOverclockingModel CreateOverclockingModel()
    {
        return new CpuOverclockingModelBuilder()
            .WithCoreClockLock(100)
            .WithCoreVoltage(50)
            .Build();
    }

    private CpuOverclocking CreateOverclocking()
    {
        return new CpuOverclockingBuilder()
            .WithCoreClockLock(100)
            .WithCoreVoltage(50)
            .Build();
    }

    private static class PresetTestsCaseSources
    {
        public static IEnumerable<List<Preset>> CorePresetLists
        {
            get
            {
                yield return
                [
                    new PresetBuilder()
                        .WithOverclocking(() =>
                        {
                            return new CpuOverclockingBuilder()
                                .WithCoreClockLock(80)
                                .WithCoreVoltage(30)
                                .Build();
                        })
                        .Build(),
                    new PresetBuilder()
                        .WithOverclocking(() =>
                        {
                            return new CpuOverclockingBuilder()
                                .WithCoreClockLock(90)
                                .WithCoreVoltage(40)
                                .Build();
                        })
                        .Build(),
                    new PresetBuilder()
                        .WithOverclocking(() =>
                        {
                            return new CpuOverclockingBuilder()
                                .WithCoreClockLock(100)
                                .WithCoreVoltage(50)
                                .Build();
                        })
                        .Build(),
                ];
            }
        }
    }
}
