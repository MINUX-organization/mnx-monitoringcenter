using MNX.MonitoringCenter.Management.Contracts.Overclocking.Gpu;
using MNX.MonitoringCenter.Management.Contracts.Overclocking.Gpu.Fan;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu.Fan;
using MNX.MonitoringCenter.Management.Tests.Service.Assertions.Overclocking;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders.Overclockings;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders.Overclockings.Fan;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.Overclockings;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.Overclockings.Fans;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Infrastructure.FanOverclocking.Model;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Overclocking.Model;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Overclocking.Model.Gpu;
using Moq;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests.Overclocking.Model;

[TestFixture]
public sealed class AmdGpuOverclockingModelMapperTests
{
    private Mock<IFanOverclockingModelMapper<
        FanOverclockingWithTargetSpeedModel, FanOverclockingWithTargetSpeed>> _fanOverclockingMapper;
    private FanOverclockingModelMapperRegistry _fanOverclockingModelMapperRegistry;
    private AmdGpuOverclockingModelMapper _amdGpuOverclockingModelMapper;

    [SetUp]
    public void SetUp()
    {
        _fanOverclockingMapper = new Mock<IFanOverclockingModelMapper<
            FanOverclockingWithTargetSpeedModel, FanOverclockingWithTargetSpeed>>();
        _fanOverclockingModelMapperRegistry = new FanOverclockingModelMapperRegistry();
        _fanOverclockingModelMapperRegistry.Register(_fanOverclockingMapper.Object);
        _amdGpuOverclockingModelMapper = new AmdGpuOverclockingModelMapper(_fanOverclockingModelMapperRegistry);
    }

    [Test]
    public void MapToCoreEntity_ValidModel_ReturnsCoreEntity()
    {
        // Arrange

        var fanOverclockingWithTargetSpeedModel = new FanOverclockingWithTargetSpeedModelBuilder().Build();
        var amdGpuOverclockingModel = CreateAmdGpuOverclockingModel(fanOverclockingWithTargetSpeedModel);

        _fanOverclockingMapper.Setup(x => x.MapToCoreEntity(fanOverclockingWithTargetSpeedModel))
            .Returns(new FanOverclockingWithTargetSpeedBuilder().Build());


        // Act

        var mappedAmdGpuOverclocking = _amdGpuOverclockingModelMapper.MapToCoreEntity(amdGpuOverclockingModel);


        // Assert

        mappedAmdGpuOverclocking.ShouldBeEquivalentTo(amdGpuOverclockingModel);

        _fanOverclockingMapper.Verify(x => x.MapToCoreEntity(fanOverclockingWithTargetSpeedModel), Times.Once);
    }

    [Test]
    public void MapToCoreEntity_ValidModelAndOriginalCoreEntity_ReturnsCoreEntity()
    {
        // Arrange

        var fanOverclockingWithTargetSpeedModel = new FanOverclockingWithTargetSpeedModelBuilder().Build();
        var amdGpuOverclockingModel = CreateAmdGpuOverclockingModel(fanOverclockingWithTargetSpeedModel);
        var originalGpuOverclocking = CreateAmdGpuOverclocking();

        _fanOverclockingMapper.Setup(x => x.MapToCoreEntity(
                fanOverclockingWithTargetSpeedModel, originalGpuOverclocking.FanOverclocking.Id))
            .Returns(new FanOverclockingWithTargetSpeedBuilder().WithId(originalGpuOverclocking.FanOverclocking.Id).Build());


        // Act

        var mappedAmdGpuOverclocking = _amdGpuOverclockingModelMapper.MapToCoreEntity(amdGpuOverclockingModel, originalGpuOverclocking);


        // Assert

        mappedAmdGpuOverclocking.ShouldBeEquivalentTo(amdGpuOverclockingModel, originalGpuOverclocking.Id);

        _fanOverclockingMapper.Verify(x => x.MapToCoreEntity(
            fanOverclockingWithTargetSpeedModel, originalGpuOverclocking.FanOverclocking.Id), Times.Once);
    }

    [Test]
    public void MapToModel_ValidCoreEntity_ReturnsModel()
    {
        // Arrange

        var fanOverclockingWithTargetSpeed = new FanOverclockingWithTargetSpeedBuilder().Build();
        var amdGpuOverclocking = CreateAmdGpuOverclocking();

        _fanOverclockingMapper.Setup(x => x.MapToModel(fanOverclockingWithTargetSpeed))
            .Returns(new FanOverclockingWithTargetSpeedModelBuilder().Build());


        // Act

        var mappedAmdGpuOverclockingModel = _amdGpuOverclockingModelMapper.MapToModel(amdGpuOverclocking);


        // Assert

        mappedAmdGpuOverclockingModel.ShouldBeEquivalentTo(amdGpuOverclocking);

        _fanOverclockingMapper.Verify(x => x.MapToModel(fanOverclockingWithTargetSpeed), Times.Once);
    }

    private static AmdGpuOverclockingModel CreateAmdGpuOverclockingModel(IFanOverclockingModel fanOverclockingModel)
    {
        return new AmdGpuOverclockingModelBuilder()
            .WithFanOverclocking(() => fanOverclockingModel)
            .WithPowerLimit(10)
            .WithCoreClockLock(2650)
            .WithCoreClockState(1)
            .WithCoreVoltage(1125)
            .WithCoreVoltageOffset(-25)
            .WithMemoryClockLock(2100)
            .WithMemoryClockState(1)
            .WithMemoryVoltage(1350)
            .WithMemoryControllerVoltage(1100)
            .WithEnhancedOverclock()
            .WithSocFrequency(1200)
            .WithSocVoltage(1050)
            .Build();
    }

    private static AmdGpuOverclocking CreateAmdGpuOverclocking()
    {
        return new AmdGpuOverclockingBuilder()
            .WithPowerLimit(10)
            .WithCoreClockLock(2650)
            .WithCoreClockState(1)
            .WithCoreVoltage(1125)
            .WithCoreVoltageOffset(-25)
            .WithMemoryClockLock(2100)
            .WithMemoryClockState(1)
            .WithMemoryVoltage(1350)
            .WithMemoryControllerVoltage(1100)
            .WithEnhancedOverclock()
            .WithSocFrequency(1200)
            .WithSocVoltage(1050)
            .Build();
    }
}
