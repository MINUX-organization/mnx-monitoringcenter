using MNX.MonitoringCenter.Management.Contracts.Overclocking.Gpu.Fan;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu.Fan;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders.Overclockings.Fan;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.Overclockings.Fans;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Fan.Model;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Overclocking.Model;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests.Fan.Model;

[TestFixture]
public sealed class FanLinearDependenceModelMapperTests
{
    private IFanOverclockingModelMapper<FanOverclockingWithLinearDependenceModel, FanOverclockingWithLinearDependence>
        _fanOverclockingWithLinearDependenceMapper;

    [SetUp]
    public void SetUp()
    {
        _fanOverclockingWithLinearDependenceMapper = new FanLinearDependenceModelMapper();
    }

    [TestCaseSource(typeof(FanOverclockingTestCases), nameof(FanOverclockingTestCases.FanOverclockingModels))]
    public void MapToCoreEntity_ValidModel_ReturnsCoreEntity(FanOverclockingWithLinearDependenceModel fanOverclockingModel)
    {
        // Arrange
        // Act

        var mappedFanOverclocking = _fanOverclockingWithLinearDependenceMapper.MapToCoreEntity(fanOverclockingModel);


        // Assert

        Assert.That(mappedFanOverclocking, Is.Not.Null);
        Assert.That(mappedFanOverclocking, Is.TypeOf<FanOverclockingWithLinearDependence>());
        Assert.Multiple(() =>
        {
            Assert.That(mappedFanOverclocking.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(mappedFanOverclocking.Type, Is.EqualTo(fanOverclockingModel.FanOverclockingType));

            var fanOverclockingCore = (FanOverclockingWithLinearDependence)mappedFanOverclocking;
            Assert.That(fanOverclockingCore.TargetPoints.Select(x => (x.FanSpeedValueTarget, x.TemperatureValueTarget)),
                Is.EqualTo(fanOverclockingModel.TargetPoints.Select(x => (x.FanSpeedValueTarget, x.TemperatureValueTarget))));
        });
    }

    [TestCaseSource(typeof(FanOverclockingTestCases), nameof(FanOverclockingTestCases.FanOverclockingModels))]
    public void MapToCoreEntity_ValidModelAndId_ReturnsCoreEntity(FanOverclockingWithLinearDependenceModel fanOverclockingModel)
    {
        // Arrange

        var fanOverclockingId = Guid.NewGuid();


        // Act

        var mappedFanOverclocking = _fanOverclockingWithLinearDependenceMapper.MapToCoreEntity(fanOverclockingModel, fanOverclockingId);


        // Assert

        Assert.That(mappedFanOverclocking, Is.Not.Null);
        Assert.That(mappedFanOverclocking, Is.TypeOf<FanOverclockingWithLinearDependence>());
        Assert.Multiple(() =>
        {
            Assert.That(mappedFanOverclocking.Id, Is.EqualTo(fanOverclockingId));
            Assert.That(mappedFanOverclocking.Type, Is.EqualTo(fanOverclockingModel.FanOverclockingType));

            var fanOverclockingCore = (FanOverclockingWithLinearDependence)mappedFanOverclocking;
            Assert.That(fanOverclockingCore.TargetPoints.Select(x => (x.FanSpeedValueTarget, x.TemperatureValueTarget)),
                Is.EqualTo(fanOverclockingModel.TargetPoints.Select(x => (x.FanSpeedValueTarget, x.TemperatureValueTarget))));
        });
    }

    [TestCaseSource(typeof(FanOverclockingTestCases), nameof(FanOverclockingTestCases.FanOverclockings))]
    public void MapToModel_ValidCoreEntity_ReturnsModel(FanOverclockingWithLinearDependence fanOverclockingCore)
    {
        // Arrange
        // Act

        var mappedFanOverclocking = _fanOverclockingWithLinearDependenceMapper.MapToModel(fanOverclockingCore);


        // Assert

        Assert.That(mappedFanOverclocking, Is.Not.Null);
        Assert.That(mappedFanOverclocking, Is.TypeOf<FanOverclockingWithLinearDependenceModel>());
        Assert.Multiple(() =>
        {
            Assert.That(mappedFanOverclocking.FanOverclockingType, Is.EqualTo(fanOverclockingCore.Type));

            var fanOverclockingModel = (FanOverclockingWithLinearDependenceModel)mappedFanOverclocking;
            Assert.That(fanOverclockingModel.TargetPoints.Select(x => (x.FanSpeedValueTarget, x.TemperatureValueTarget)),
                Is.EqualTo(fanOverclockingCore.TargetPoints.Select(x => (x.FanSpeedValueTarget, x.TemperatureValueTarget))));
        });
    }

    private static class FanOverclockingTestCases
    {
        public static IEnumerable<FanOverclockingWithLinearDependenceModel> FanOverclockingModels
        {
            get
            {
                yield return new FanOverclockingWithLinearDependenceModelBuilder()
                    .AddTargetPoint(targetPoint =>
                        targetPoint.WithFanSpeedValueTarget(30)
                                   .WithTemperatureValueTarget(25))
                    .AddTargetPoint(targetPoint =>
                        targetPoint.WithFanSpeedValueTarget(45)
                                   .WithTemperatureValueTarget(40))
                    .AddTargetPoint(targetPoint =>
                        targetPoint.WithFanSpeedValueTarget(60)
                                   .WithTemperatureValueTarget(50))
                    .AddTargetPoint(targetPoint =>
                        targetPoint.WithFanSpeedValueTarget(75)
                                   .WithTemperatureValueTarget(65))
                    .AddTargetPoint(targetPoint =>
                        targetPoint.WithFanSpeedValueTarget(100)
                                   .WithTemperatureValueTarget(75))
                    .Build();
            }
        }

        public static IEnumerable<FanOverclockingWithLinearDependence> FanOverclockings
        {
            get
            {
                yield return new FanOverclockingWithLinearDependenceBuilder()
                    .AddTargetPoint(targetPoint =>
                        targetPoint.WithFanSpeedValueTarget(30)
                                   .WithTemperatureValueTarget(25))
                    .AddTargetPoint(targetPoint =>
                        targetPoint.WithFanSpeedValueTarget(45)
                                   .WithTemperatureValueTarget(40))
                    .AddTargetPoint(targetPoint =>
                        targetPoint.WithFanSpeedValueTarget(60)
                                   .WithTemperatureValueTarget(50))
                    .AddTargetPoint(targetPoint =>
                        targetPoint.WithFanSpeedValueTarget(75)
                                   .WithTemperatureValueTarget(65))
                    .AddTargetPoint(targetPoint =>
                        targetPoint.WithFanSpeedValueTarget(100)
                                   .WithTemperatureValueTarget(75))
                    .Build();
            }
        }
    }
}
