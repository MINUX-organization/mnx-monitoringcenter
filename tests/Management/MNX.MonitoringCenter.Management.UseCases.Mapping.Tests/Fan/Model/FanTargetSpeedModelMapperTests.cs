using MNX.MonitoringCenter.Management.Contracts.Overclocking.Gpu.Fan;
using MNX.MonitoringCenter.Management.Core.Overclocking.Enums;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu.Fan;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders.Overclockings.Fan;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.Overclockings.Fans;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Fan.Model;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Overclocking.Model;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests.Fan.Model;

[TestFixture]
public sealed class FanTargetSpeedModelMapperTests
{
    private IFanOverclockingModelMapper<FanOverclockingWithTargetSpeedModel, FanOverclockingWithTargetSpeed>
        _fanOverclockingWithTargetSpeedMapper;

    [SetUp]
    public void SetUp()
    {
        _fanOverclockingWithTargetSpeedMapper = new FanTargetSpeedModelMapper();
    }

    [Test]
    public void MapToCoreEntity_ValidModel_ReturnsCoreEntity()
    {
        // Arrange

        var fanOverclockingModel = CreateFanOverclockingWithTargetSpeedModel();


        // Act

        var mappedFanOverclocking = _fanOverclockingWithTargetSpeedMapper.MapToCoreEntity(fanOverclockingModel);


        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(mappedFanOverclocking, Is.Not.Null);
            Assert.That(mappedFanOverclocking.Type, Is.EqualTo(fanOverclockingModel.FanOverclockingType));
            Assert.That(mappedFanOverclocking, Is.TypeOf<FanOverclockingWithTargetSpeed>());


            var fanOverclocking = (FanOverclockingWithTargetSpeed)mappedFanOverclocking;
            Assert.That(fanOverclocking.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(fanOverclocking.TargetSpeed, Is.EqualTo(fanOverclockingModel.TargetSpeed));
        });
    }

    [Test]
    public void MapToCoreEntity_ValidModelAndId_ReturnsCoreEntity()
    {
        // Arrange

        var fanOverclockingId = Guid.NewGuid();
        var fanOverclockingModel = CreateFanOverclockingWithTargetSpeedModel();


        // Act

        var mappedFanOverclocking = _fanOverclockingWithTargetSpeedMapper.MapToCoreEntity(fanOverclockingModel, fanOverclockingId);


        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(mappedFanOverclocking, Is.Not.Null);
            Assert.That(mappedFanOverclocking.Type, Is.EqualTo(FanOverclockingType.TargetSpeed));
            Assert.That(mappedFanOverclocking, Is.TypeOf<FanOverclockingWithTargetSpeed>());


            var fanOverclocking = (FanOverclockingWithTargetSpeed)mappedFanOverclocking;
            Assert.That(fanOverclocking.Id, Is.EqualTo(fanOverclockingId));
            Assert.That(fanOverclocking.TargetSpeed, Is.EqualTo(fanOverclockingModel.TargetSpeed));
        });
    }

    [Test]
    public void MapToModel_ValidCoreEntity_ReturnsModel()
    {
        // Arrange

        var fanOverclockingCore = CreateFanOverclockingWithTargetSpeed();


        // Act

        var mappedFanOverclockingModel = _fanOverclockingWithTargetSpeedMapper.MapToModel(fanOverclockingCore);


        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(mappedFanOverclockingModel, Is.Not.Null);
            Assert.That(mappedFanOverclockingModel.FanOverclockingType, Is.EqualTo(fanOverclockingCore.Type));
            Assert.That(mappedFanOverclockingModel, Is.TypeOf<FanOverclockingWithTargetSpeedModel>());

            var fanOverclockingModel = (FanOverclockingWithTargetSpeedModel)mappedFanOverclockingModel;
            Assert.That(fanOverclockingModel.TargetSpeed, Is.EqualTo(fanOverclockingCore.TargetSpeed));
        });
    }

    private FanOverclockingWithTargetSpeedModel CreateFanOverclockingWithTargetSpeedModel()
    {
        return new FanOverclockingWithTargetSpeedModelBuilder()
            .WithTargetSpeed(100)
            .Build();
    }

    private FanOverclockingWithTargetSpeed CreateFanOverclockingWithTargetSpeed()
    {
        return new FanOverclockingWithTargetSpeedBuilder()
            .WithTargetSpeed(100)
            .Build();
    }
}
