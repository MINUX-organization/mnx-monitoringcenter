using EasyNetQ;
using EasyNetQ.Topology;
using Microsoft.Extensions.DependencyInjection;
using MNX.MonitoringCenter.Traffic.Contracts.Bus;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Mining;
using MNX.MonitoringCenter.Traffic.Observers.Abstractions;
using Moq;

namespace MNX.MonitoringCenter.RigsApi.Service.Tests.RigDynamicIndicatorsConsumer;

internal class RigDynamicIndicatorsConsumerTests
{
    private IMessageBrokerFixture _broker;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _broker = GlobalMessageBrokerFixture.Broker;
    }

    [TestCaseSource(typeof(RigDynamicIndicatorsTestCaseSource), nameof(RigDynamicIndicatorsTestCaseSource.Indicators))]
    public async Task Consume_ShouldCallAggregator(RigDynamicIndicators message)
    {
        // Arrange

        var aggregatorMock = new Mock<IUserRigsObserverAggregator>();
        var services = new ServiceCollection();
        var connectionString = $"host={_broker.Host};port={_broker.Port};username=guest;password=guest";
        var bus = RabbitHutch.CreateBus(connectionString).Advanced;

        services.AddSingleton(bus);
        services.AddSingleton(aggregatorMock.Object);
        services.AddSingleton<Consumers.RigDynamicIndicatorsConsumer>();

        var provider = services.BuildServiceProvider();
        var consumer = provider.GetRequiredService<Consumers.RigDynamicIndicatorsConsumer>();
        await consumer.StartAsync(CancellationToken.None);

        var publishing = new Message<RigDynamicIndicators>(message);


        // Act

        await bus.PublishAsync(
            exchange: new Exchange("MNX.MonitoringCenter.Traffic.Contracts.Bus.RigDynamicIndicators, MNX.MonitoringCenter.Traffic.Contracts.Bus"),
            routingKey: "",
            mandatory: false,
            message: publishing);

        await Task.Delay(1000);


        // Assert

        aggregatorMock.Verify(x => x.SetIndicators(
            message.UserId, It.IsAny<RigDynamicIndicators>()), Times.Once);

        await consumer.StopAsync(CancellationToken.None);
    }

    private sealed class RigDynamicIndicatorsTestCaseSource
    {
        public static IEnumerable<RigDynamicIndicators> Indicators
        {
            get
            {
                yield return new RigDynamicIndicators
                {
                    RigId = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    BootedUpTimeInSeconds = 104000,
                    Devices =
                    [
                        new GpuDynamicIndicators()
                        {
                            MemoryTemperature = 56,
                            CoreTemperature = 67,
                            FanSpeed = 77,
                            MiningUpTimeInSeconds = 112233,
                            MiningState = MiningState.Active,
                        },
                        new CpuDynamicIndicators()
                        {
                            Temperature = 76,
                            MiningState = MiningState.Active,
                        }
                    ]
                };
            }
        }
    }
}
