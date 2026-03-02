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

    [TestCaseSource(typeof(FanOverclockingTestCases), nameof(FanOverclockingTestCases.FanOverclockingModels))]
    public void MapToCoreEntity_ValidModel_ReturnsCoreEntity(FanOverclockingWithTargetSpeedModel fanOverclockingModel)
    {
        // Arrange
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

    [TestCaseSource(typeof(FanOverclockingTestCases), nameof(FanOverclockingTestCases.FanOverclockingModels))]
    public void MapToCoreEntity_ValidModelAndId_ReturnsCoreEntity(FanOverclockingWithTargetSpeedModel fanOverclockingModel)
    {
        // Arrange

        var fanOverclockingId = Guid.NewGuid();


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

    [TestCaseSource(typeof(FanOverclockingTestCases), nameof(FanOverclockingTestCases.FanOverclockings))]
    public void MapToModel_ValidCoreEntity_ReturnsModel(FanOverclockingWithTargetSpeed fanOverclockingCore)
    {
        // Arrange
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

    private static class FanOverclockingTestCases
    {
        public static IEnumerable<FanOverclockingWithTargetSpeedModel> FanOverclockingModels
        {
            get
            {
                yield return new FanOverclockingWithTargetSpeedModelBuilder()
                    .WithTargetSpeed(100)
                    .Build();
                yield return new FanOverclockingWithTargetSpeedModelBuilder()
                    .WithTargetSpeed(50)
                    .Build();
                yield return new FanOverclockingWithTargetSpeedModelBuilder()
                    .Build();
            }
        }

        public static IEnumerable<FanOverclockingWithTargetSpeed> FanOverclockings
        {
            get
            {
                yield return new FanOverclockingWithTargetSpeedBuilder()
                    .WithTargetSpeed(100)
                    .Build();
                yield return new FanOverclockingWithTargetSpeedBuilder()
                    .WithTargetSpeed(50)
                    .Build();
                yield return new FanOverclockingWithTargetSpeedBuilder()
                    .Build();
            }
        }
    }
}
