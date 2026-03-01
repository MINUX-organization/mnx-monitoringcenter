using MNX.MonitoringCenter.Management.Agent.Commands.Overclocking.Fan;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.Overclockings.Fans;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Fan.Agent;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Overclocking.Agent;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests.Fan.Agent;

using AgentFanOverclocking = FanOverclockingWithLinearDependence;
using CoreFanOverclocking = Core.Overclocking.Gpu.Fan.FanOverclockingWithLinearDependence;
using AgentFanGraphicPoint = FanGraphicPoint;
using CoreFanGraphicPoint = Core.Overclocking.Gpu.Fan.FanGraphicPoint;

[TestFixture]
public sealed class FanLinearDependenceAgentMapperTests
{
    private IFanOverclockingAgentMapper<AgentFanOverclocking, CoreFanOverclocking> _fanLinearDependenceAgentMapper;

    [SetUp]
    public void SetUp()
    {
        _fanLinearDependenceAgentMapper = new FanLinearDependenceAgentMapper();
    }

    [Test]
    public void MapToModel_ValidCoreEntity_ReturnsAgentModel()
    {
        // Arrange

        var coreFanOverclocking = new FanOverclockingWithLinearDependenceBuilder()
            .AddTargetPoint(point =>
                point.WithPointIndex(0)
                     .WithFanSpeedValueTarget(40)
                     .WithTemperatureValueTarget(30))
            .AddTargetPoint(point =>
                point.WithPointIndex(1)
                     .WithFanSpeedValueTarget(55)
                     .WithTemperatureValueTarget(45))
            .AddTargetPoint(point =>
                point.WithPointIndex(2)
                     .WithFanSpeedValueTarget(70)
                     .WithTemperatureValueTarget(60))
            .AddTargetPoint(point =>
                point.WithPointIndex(3)
                     .WithFanSpeedValueTarget(85)
                     .WithTemperatureValueTarget(75))
            .Build();


        // Act

        var mappedAgentFanOverclocking = _fanLinearDependenceAgentMapper.MapToModel(coreFanOverclocking);


        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(mappedAgentFanOverclocking, Is.Not.Null);
            Assert.That(mappedAgentFanOverclocking, Is.TypeOf<AgentFanOverclocking>());

            var agentFanOverclocking = (AgentFanOverclocking)mappedAgentFanOverclocking;
            AssertTargets(coreFanOverclocking.TargetPoints, agentFanOverclocking.TargetPoints);
        });
    }

    private void AssertTargets(CoreFanGraphicPoint[] expected, AgentFanGraphicPoint[] checking)
    {
        Assert.That(checking, Has.Length.EqualTo(expected.Length));
        for (var i = 0; i < expected.Length; i++)
        {
            Assert.Multiple(() =>
            {
                Assert.That(checking[i].PointIndex, Is.EqualTo(expected[i].PointIndex));
                Assert.That(checking[i].FanSpeedValueTarget, Is.EqualTo(expected[i].FanSpeedValueTarget));
                Assert.That(checking[i].TemperatureValueTarget, Is.EqualTo(expected[i].TemperatureValueTarget));
            });
        }
    }
}
