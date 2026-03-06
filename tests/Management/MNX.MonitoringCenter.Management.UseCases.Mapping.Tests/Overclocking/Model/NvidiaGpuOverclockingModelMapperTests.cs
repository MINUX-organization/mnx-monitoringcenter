using MNX.MonitoringCenter.Management.Contracts.Overclocking.Gpu;
using MNX.MonitoringCenter.Management.Contracts.Overclocking.Gpu.Fan;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu.Fan;
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

        Assert.That(mappedNvidiaCpuOverclocking, Is.Not.Null);
        Assert.That(mappedNvidiaCpuOverclocking, Is.TypeOf<NvidiaGpuOverclocking>());
        Assert.Multiple(() =>
        {
            Assert.That(mappedNvidiaCpuOverclocking.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(mappedNvidiaCpuOverclocking.TargetDeviceType, Is.EqualTo(nvidiaGpuOverclockingModel.TargetDeviceType));

            var nvidiaGpuOverclocking = (NvidiaGpuOverclocking)mappedNvidiaCpuOverclocking;
            Assert.That(nvidiaGpuOverclocking.CoreClockLock, Is.EqualTo(nvidiaGpuOverclockingModel.CoreClockLock));
            Assert.That(nvidiaGpuOverclocking.CoreClockOffset, Is.EqualTo(nvidiaGpuOverclockingModel.CoreClockOffset));
            Assert.That(nvidiaGpuOverclocking.PowerLimit, Is.EqualTo(nvidiaGpuOverclockingModel.PowerLimit));
            Assert.That(nvidiaGpuOverclocking.CoreVoltage, Is.EqualTo(nvidiaGpuOverclockingModel.CoreVoltage));
            Assert.That(nvidiaGpuOverclocking.CoreVoltageOffset, Is.EqualTo(nvidiaGpuOverclockingModel.CoreVoltageOffset));
            Assert.That(nvidiaGpuOverclocking.MemoryClockLock, Is.EqualTo(nvidiaGpuOverclockingModel.MemoryClockLock));
            Assert.That(nvidiaGpuOverclocking.MemoryClockOffset, Is.EqualTo(nvidiaGpuOverclockingModel.MemoryClockOffset));
            Assert.That(nvidiaGpuOverclocking.MemoryVoltage, Is.EqualTo(nvidiaGpuOverclockingModel.MemoryVoltage));
            Assert.That(nvidiaGpuOverclocking.MemoryVoltageOffset, Is.EqualTo(nvidiaGpuOverclockingModel.MemoryVoltageOffset));
        });

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

        Assert.That(mappedGpuOverclocking, Is.Not.Null);
        Assert.That(mappedGpuOverclocking, Is.TypeOf<NvidiaGpuOverclocking>());
        Assert.Multiple(() =>
        {
            Assert.That(mappedGpuOverclocking.Id, Is.EqualTo(originalGpuOverclocking.Id));
            Assert.That(mappedGpuOverclocking.TargetDeviceType, Is.EqualTo(nvidiaGpuOverclockingModel.TargetDeviceType));

            var nvidiaGpuOverclocking = (NvidiaGpuOverclocking)mappedGpuOverclocking;
            Assert.That(nvidiaGpuOverclocking.PowerLimit, Is.EqualTo(nvidiaGpuOverclockingModel.PowerLimit));
            Assert.That(nvidiaGpuOverclocking.CoreClockLock, Is.EqualTo(nvidiaGpuOverclockingModel.CoreClockLock));
            Assert.That(nvidiaGpuOverclocking.CoreClockOffset, Is.EqualTo(nvidiaGpuOverclockingModel.CoreClockOffset));
            Assert.That(nvidiaGpuOverclocking.CoreVoltage, Is.EqualTo(nvidiaGpuOverclockingModel.CoreVoltage));
            Assert.That(nvidiaGpuOverclocking.CoreVoltageOffset, Is.EqualTo(nvidiaGpuOverclockingModel.CoreVoltageOffset));
            Assert.That(nvidiaGpuOverclocking.MemoryClockLock, Is.EqualTo(nvidiaGpuOverclockingModel.MemoryClockLock));
            Assert.That(nvidiaGpuOverclocking.MemoryClockOffset, Is.EqualTo(nvidiaGpuOverclockingModel.MemoryClockOffset));
            Assert.That(nvidiaGpuOverclocking.MemoryVoltage, Is.EqualTo(nvidiaGpuOverclockingModel.MemoryVoltage));
            Assert.That(nvidiaGpuOverclocking.MemoryVoltageOffset, Is.EqualTo(nvidiaGpuOverclockingModel.MemoryVoltageOffset));
        });

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

        Assert.That(mappedNvidiaGpuOverclockingModel, Is.Not.Null);
        Assert.That(mappedNvidiaGpuOverclockingModel, Is.TypeOf<NvidiaGpuOverclockingModel>());
        Assert.Multiple(() =>
        {
            Assert.That(mappedNvidiaGpuOverclockingModel.TargetDeviceType, Is.EqualTo(nvidiaGpuOverclocking.TargetDeviceType));

            var nvidiaGpuOverclockingModel = (NvidiaGpuOverclockingModel)mappedNvidiaGpuOverclockingModel;
            Assert.That(nvidiaGpuOverclockingModel.PowerLimit, Is.EqualTo(nvidiaGpuOverclocking.PowerLimit));
            Assert.That(nvidiaGpuOverclockingModel.CoreClockLock, Is.EqualTo(nvidiaGpuOverclocking.CoreClockLock));
            Assert.That(nvidiaGpuOverclockingModel.CoreClockOffset, Is.EqualTo(nvidiaGpuOverclocking.CoreClockOffset));
            Assert.That(nvidiaGpuOverclockingModel.CoreVoltage, Is.EqualTo(nvidiaGpuOverclocking.CoreVoltage));
            Assert.That(nvidiaGpuOverclockingModel.CoreVoltageOffset, Is.EqualTo(nvidiaGpuOverclocking.CoreVoltageOffset));
            Assert.That(nvidiaGpuOverclockingModel.MemoryClockLock, Is.EqualTo(nvidiaGpuOverclocking.MemoryClockLock));
            Assert.That(nvidiaGpuOverclockingModel.MemoryClockOffset, Is.EqualTo(nvidiaGpuOverclocking.MemoryClockOffset));
            Assert.That(nvidiaGpuOverclockingModel.MemoryVoltage, Is.EqualTo(nvidiaGpuOverclocking.MemoryVoltage));
            Assert.That(nvidiaGpuOverclockingModel.MemoryVoltage, Is.EqualTo(nvidiaGpuOverclocking.MemoryVoltage));
        });

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
