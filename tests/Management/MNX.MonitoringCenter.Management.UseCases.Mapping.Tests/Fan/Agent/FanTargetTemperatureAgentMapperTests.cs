using MNX.MonitoringCenter.Management.Agent.Commands.Overclocking.Fan;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.Overclockings.Fans;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Fan.Agent;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Overclocking.Agent;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests.Fan.Agent;

using AgentFanOverclocking = FanOverclockingWithTargetTemperature;
using CoreFanOverclocking = Core.Overclocking.Gpu.Fan.FanOverclockingWithTargetTemperature;

[TestFixture]
public sealed class FanTargetTemperatureAgentMapperTests
{
    private IFanOverclockingAgentMapper<AgentFanOverclocking, CoreFanOverclocking> _fanTargetTemperatureMapper;

    [SetUp]
    public void SetUp()
    {
        _fanTargetTemperatureMapper = new FanTargetTemperatureAgentMapper();
    }

    [Test]
    public void MapToModel_ValidCoreEntity_ReturnsAgentModel()
    {
        // Arrange

        var coreFanOverclocking = new FanOverclockingWithTargetTemperatureBuilder()
            .WithTargetCoreTemperature(70)
            .WithTargetMemoryTemperature(55)
            .WithMaxTargetSpeed(90)
            .WithMinTargetSpeed(40)
            .Build();


        // Act

        var mappedAgentFanOverclocking = _fanTargetTemperatureMapper.MapToModel(coreFanOverclocking);


        // Assert

        Assert.That(mappedAgentFanOverclocking, Is.Not.Null);
        Assert.That(mappedAgentFanOverclocking, Is.TypeOf<AgentFanOverclocking>());
        Assert.Multiple(() =>
        {
            var agentFanOverclocking = (AgentFanOverclocking)mappedAgentFanOverclocking;
            Assert.That(agentFanOverclocking.MaxTargetSpeed, Is.EqualTo(coreFanOverclocking.MaxTargetSpeed));
            Assert.That(agentFanOverclocking.MinTargetSpeed, Is.EqualTo(coreFanOverclocking.MinTargetSpeed));
            Assert.That(agentFanOverclocking.TargetCoreTemperature, Is.EqualTo(coreFanOverclocking.TargetCoreTemperature));
            Assert.That(agentFanOverclocking.TargetMemoryTemperature, Is.EqualTo(coreFanOverclocking.TargetMemoryTemperature));
        });
    }
}
