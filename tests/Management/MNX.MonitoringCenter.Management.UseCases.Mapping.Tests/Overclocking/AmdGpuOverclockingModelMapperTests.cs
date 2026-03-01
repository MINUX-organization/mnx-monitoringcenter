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

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests.Overclocking;

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

        Assert.Multiple(() =>
        {
            Assert.That(mappedAmdGpuOverclocking, Is.Not.Null);
            Assert.That(mappedAmdGpuOverclocking.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(mappedAmdGpuOverclocking.TargetDeviceType, Is.EqualTo(amdGpuOverclockingModel.TargetDeviceType));
            Assert.That(mappedAmdGpuOverclocking, Is.TypeOf<AmdGpuOverclocking>());

            var amdGpuOverclocking = (AmdGpuOverclocking)mappedAmdGpuOverclocking;
            Assert.That(amdGpuOverclocking.PowerLimit, Is.EqualTo(amdGpuOverclockingModel.PowerLimit));
            Assert.That(amdGpuOverclocking.CoreClockLock, Is.EqualTo(amdGpuOverclockingModel.CoreClockLock));
            Assert.That(amdGpuOverclocking.CoreClockState, Is.EqualTo(amdGpuOverclockingModel.CoreClockState));
            Assert.That(amdGpuOverclocking.CoreVoltage, Is.EqualTo(amdGpuOverclockingModel.CoreVoltage));
            Assert.That(amdGpuOverclocking.CoreVoltageOffset, Is.EqualTo(amdGpuOverclockingModel.CoreVoltageOffset));
            Assert.That(amdGpuOverclocking.MemoryClockLock, Is.EqualTo(amdGpuOverclockingModel.MemoryClockLock));
            Assert.That(amdGpuOverclocking.MemoryClockState, Is.EqualTo(amdGpuOverclockingModel.MemoryClockState));
            Assert.That(amdGpuOverclocking.MemoryVoltage, Is.EqualTo(amdGpuOverclockingModel.MemoryVoltage));
            Assert.That(amdGpuOverclocking.MemoryControllerVoltage, Is.EqualTo(amdGpuOverclockingModel.MemoryControllerVoltage));
            Assert.That(amdGpuOverclocking.MemoryTweak, Is.EqualTo(amdGpuOverclockingModel.MemoryTweak));
            Assert.That(amdGpuOverclocking.EnhancedOverclock, Is.EqualTo(amdGpuOverclockingModel.EnhancedOverclock));
            Assert.That(amdGpuOverclocking.AlternativeDownVoltage, Is.EqualTo(amdGpuOverclockingModel.AlternativeDownVoltage));
            Assert.That(amdGpuOverclocking.SocFrequency, Is.EqualTo(amdGpuOverclockingModel.SocFrequency));
            Assert.That(amdGpuOverclocking.SocVoltage, Is.EqualTo(amdGpuOverclockingModel.SocVoltage));
        });

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

        Assert.Multiple(() =>
        {
            Assert.That(mappedAmdGpuOverclocking, Is.Not.Null);
            Assert.That(mappedAmdGpuOverclocking.Id, Is.EqualTo(originalGpuOverclocking.Id));
            Assert.That(mappedAmdGpuOverclocking.TargetDeviceType, Is.EqualTo(amdGpuOverclockingModel.TargetDeviceType));
            Assert.That(mappedAmdGpuOverclocking, Is.TypeOf<AmdGpuOverclocking>());

            var amdGpuOverclocking = (AmdGpuOverclocking)mappedAmdGpuOverclocking;
            Assert.That(amdGpuOverclocking.PowerLimit, Is.EqualTo(amdGpuOverclockingModel.PowerLimit));
            Assert.That(amdGpuOverclocking.CoreClockLock, Is.EqualTo(amdGpuOverclockingModel.CoreClockLock));
            Assert.That(amdGpuOverclocking.CoreClockState, Is.EqualTo(amdGpuOverclockingModel.CoreClockState));
            Assert.That(amdGpuOverclocking.CoreVoltage, Is.EqualTo(amdGpuOverclockingModel.CoreVoltage));
            Assert.That(amdGpuOverclocking.CoreVoltageOffset, Is.EqualTo(amdGpuOverclockingModel.CoreVoltageOffset));
            Assert.That(amdGpuOverclocking.MemoryClockLock, Is.EqualTo(amdGpuOverclockingModel.MemoryClockLock));
            Assert.That(amdGpuOverclocking.MemoryClockState, Is.EqualTo(amdGpuOverclockingModel.MemoryClockState));
            Assert.That(amdGpuOverclocking.MemoryVoltage, Is.EqualTo(amdGpuOverclockingModel.MemoryVoltage));
            Assert.That(amdGpuOverclocking.MemoryControllerVoltage, Is.EqualTo(amdGpuOverclockingModel.MemoryControllerVoltage));
            Assert.That(amdGpuOverclocking.MemoryTweak, Is.EqualTo(amdGpuOverclockingModel.MemoryTweak));
            Assert.That(amdGpuOverclocking.EnhancedOverclock, Is.EqualTo(amdGpuOverclockingModel.EnhancedOverclock));
            Assert.That(amdGpuOverclocking.AlternativeDownVoltage, Is.EqualTo(amdGpuOverclockingModel.AlternativeDownVoltage));
            Assert.That(amdGpuOverclocking.SocFrequency, Is.EqualTo(amdGpuOverclockingModel.SocFrequency));
            Assert.That(amdGpuOverclocking.SocVoltage, Is.EqualTo(amdGpuOverclockingModel.SocVoltage));
        });

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

        Assert.Multiple(() =>
        {
            Assert.That(mappedAmdGpuOverclockingModel, Is.Not.Null);
            Assert.That(mappedAmdGpuOverclockingModel.TargetDeviceType, Is.EqualTo(amdGpuOverclocking.TargetDeviceType));
            Assert.That(mappedAmdGpuOverclockingModel, Is.TypeOf<AmdGpuOverclockingModel>());

            var amdGpuOverclockingModel = (AmdGpuOverclockingModel)mappedAmdGpuOverclockingModel;
            Assert.That(amdGpuOverclockingModel.PowerLimit, Is.EqualTo(amdGpuOverclocking.PowerLimit));
            Assert.That(amdGpuOverclockingModel.CoreClockLock, Is.EqualTo(amdGpuOverclocking.CoreClockLock));
            Assert.That(amdGpuOverclockingModel.CoreClockState, Is.EqualTo(amdGpuOverclocking.CoreClockState));
            Assert.That(amdGpuOverclockingModel.CoreVoltage, Is.EqualTo(amdGpuOverclocking.CoreVoltage));
            Assert.That(amdGpuOverclockingModel.CoreVoltageOffset, Is.EqualTo(amdGpuOverclocking.CoreVoltageOffset));
            Assert.That(amdGpuOverclockingModel.MemoryClockLock, Is.EqualTo(amdGpuOverclocking.MemoryClockLock));
            Assert.That(amdGpuOverclockingModel.MemoryClockState, Is.EqualTo(amdGpuOverclocking.MemoryClockState));
            Assert.That(amdGpuOverclockingModel.MemoryVoltage, Is.EqualTo(amdGpuOverclocking.MemoryVoltage));
            Assert.That(amdGpuOverclockingModel.MemoryControllerVoltage, Is.EqualTo(amdGpuOverclocking.MemoryControllerVoltage));
            Assert.That(amdGpuOverclockingModel.MemoryTweak, Is.EqualTo(amdGpuOverclocking.MemoryTweak));
            Assert.That(amdGpuOverclockingModel.EnhancedOverclock, Is.EqualTo(amdGpuOverclocking.EnhancedOverclock));
            Assert.That(amdGpuOverclockingModel.AlternativeDownVoltage, Is.EqualTo(amdGpuOverclocking.AlternativeDownVoltage));
            Assert.That(amdGpuOverclockingModel.SocFrequency, Is.EqualTo(amdGpuOverclocking.SocFrequency));
            Assert.That(amdGpuOverclockingModel.SocVoltage, Is.EqualTo(amdGpuOverclocking.SocVoltage));
        });

        _fanOverclockingMapper.Verify(x => x.MapToModel(fanOverclockingWithTargetSpeed), Times.Once);
    }

    private AmdGpuOverclockingModel CreateAmdGpuOverclockingModel(IFanOverclockingModel fanOverclockingModel)
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

    private AmdGpuOverclocking CreateAmdGpuOverclocking()
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
