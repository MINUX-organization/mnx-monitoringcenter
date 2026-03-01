using MNX.MonitoringCenter.Management.Agent.Commands.Overclocking.Fan;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.Overclockings.Fans;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Fan.Agent;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Overclocking.Agent;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests.Fan.Agent;

using AgentFanOverclocking = FanOverclockingWithTargetSpeed;
using CoreFanOverclocking = Core.Overclocking.Gpu.Fan.FanOverclockingWithTargetSpeed;

[TestFixture]
public sealed class FanTargetSpeedAgentMapperTests
{
    private IFanOverclockingAgentMapper<AgentFanOverclocking, CoreFanOverclocking> _fanTargetSpeedAgentMapper;

    [SetUp]
    public void SetUp()
    {
        _fanTargetSpeedAgentMapper = new FanTargetSpeedAgentMapper();
    }

    [Test]
    public void MapToModel_ValidCoreEntity_ReturnsAgentModel()
    {
        // Arrange

        var coreFanOverclocking = new FanOverclockingWithTargetSpeedBuilder()
            .WithTargetSpeed(100)
            .Build();


        // Act

        var mappedAgentFanOverclocking = _fanTargetSpeedAgentMapper.MapToModel(coreFanOverclocking);


        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(mappedAgentFanOverclocking, Is.Not.Null);
            Assert.That(mappedAgentFanOverclocking, Is.TypeOf<AgentFanOverclocking>());

            var agentFanOverclocking = (AgentFanOverclocking)mappedAgentFanOverclocking;
            Assert.That(agentFanOverclocking.TargetSpeed, Is.EqualTo(coreFanOverclocking.TargetSpeed));
        });
    }
}
