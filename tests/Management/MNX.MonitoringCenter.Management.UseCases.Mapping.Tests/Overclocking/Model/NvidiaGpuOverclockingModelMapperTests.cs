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
public sealed class NvidiaGpuOverclockingModelMapperTests
{
    private Mock<IFanOverclockingModelMapper<
        FanOverclockingWithTargetSpeedModel, FanOverclockingWithTargetSpeed>> _fanOverclockingMapper;
    private NvidiaGpuOverclockingModelMapper _nvidiaGpuOverclockingModeMapper;
    private FanOverclockingModelMapperRegistry _fanOverclockingModelMapperRegistry;

    [SetUp]
    public void SetUp()
    {
        _fanOverclockingMapper = new Mock<IFanOverclockingModelMapper<FanOverclockingWithTargetSpeedModel, FanOverclockingWithTargetSpeed>>();
        _fanOverclockingModelMapperRegistry = new FanOverclockingModelMapperRegistry();
        _fanOverclockingModelMapperRegistry.Register(_fanOverclockingMapper.Object);
        _nvidiaGpuOverclockingModeMapper = new NvidiaGpuOverclockingModelMapper(_fanOverclockingModelMapperRegistry);
    }

    [Test]
    public void MapToCoreEntity_ValidModel_ReturnsNvidiaGpuOverclocking()
    {
        // Arrange

        var fanOverclockingWithTargetSpeedModel = new FanOverclockingWithTargetSpeedModelBuilder().Build();
        var nvidiaGpuOverclockingModel = CreateNvidiaGpuOverclockingModel(fanOverclockingWithTargetSpeedModel);

        _fanOverclockingMapper.Setup(x => x.MapToCoreEntity(fanOverclockingWithTargetSpeedModel))
            .Returns(new FanOverclockingWithTargetSpeedBuilder().Build());


        // Act

        var mappedNvidiaCpuOverclocking = _nvidiaGpuOverclockingModeMapper.MapToCoreEntity(nvidiaGpuOverclockingModel);


        // Assert

        mappedNvidiaCpuOverclocking.ShouldBeEquivalentTo(nvidiaGpuOverclockingModel);

        _fanOverclockingMapper.Verify(x => x.MapToCoreEntity(fanOverclockingWithTargetSpeedModel), Times.Once);
    }

    [Test]
    public void MapToCoreEntity_ValidModelAndOriginalCoreEntity_ReturnsNvidiaGpuOverclocking()
    {
        // Arrange

        var fanOverclockingWithTargetSpeedModel = new FanOverclockingWithTargetSpeedModelBuilder().Build();
        var nvidiaGpuOverclockingModel = CreateNvidiaGpuOverclockingModel(fanOverclockingWithTargetSpeedModel);

        var originalGpuOverclocking = CreateNvidiaGpuOverclocking();

        _fanOverclockingMapper.Setup(x => x
            .MapToCoreEntity(fanOverclockingWithTargetSpeedModel, originalGpuOverclocking.FanOverclocking.Id))
            .Returns(new FanOverclockingWithTargetSpeedBuilder().WithId(originalGpuOverclocking.FanOverclocking.Id)
                                                                .Build());


        // Act

        var mappedGpuOverclocking = _nvidiaGpuOverclockingModeMapper
            .MapToCoreEntity(nvidiaGpuOverclockingModel, originalGpuOverclocking);


        // Assert

        mappedGpuOverclocking.ShouldBeEquivalentTo(nvidiaGpuOverclockingModel);

        _fanOverclockingMapper.Verify(x => x.MapToCoreEntity(
            fanOverclockingWithTargetSpeedModel,
            originalGpuOverclocking.FanOverclocking.Id), Times.Once);
    }

    [Test]
    public void MapToModel_ValidCoreEntity_ReturnsNvidiaGpuOverclockingModel()
    {
        // Arrange

        var fanOverclockingWithTargetSpeed = new FanOverclockingWithTargetSpeedBuilder().Build();
        var nvidiaGpuOverclocking = CreateNvidiaGpuOverclocking();

        _fanOverclockingMapper.Setup(x => x.MapToModel(fanOverclockingWithTargetSpeed))
            .Returns(new FanOverclockingWithTargetSpeedModelBuilder().Build());


        // Act

        var mappedNvidiaGpuOverclockingModel = _nvidiaGpuOverclockingModeMapper.MapToModel(nvidiaGpuOverclocking);


        // Assert

        mappedNvidiaGpuOverclockingModel.ShouldBeEquivalentTo(nvidiaGpuOverclocking);

        _fanOverclockingMapper.Verify(x => x.MapToModel(fanOverclockingWithTargetSpeed), Times.Once);
    }

    private NvidiaGpuOverclockingModel CreateNvidiaGpuOverclockingModel(IFanOverclockingModel fanOverclockingModel)
    {
        return new NvidiaGpuOverclockingModelBuilder()
            .WithFanOverclocking(() => fanOverclockingModel)
            .WithCoreClockOffset(120)
            .WithCoreVoltageOffset(-50)
            .WithMemoryClockOffset(700)
            .WithPowerLimit(105)
            .Build();
    }

    private NvidiaGpuOverclocking CreateNvidiaGpuOverclocking()
    {
        return new NvidiaGpuOverclockingBuilder()
            .WithCoreClockOffset(120)
            .WithCoreVoltageOffset(-50)
            .WithMemoryClockOffset(700)
            .WithPowerLimit(105)
            .Build();
    }
}
