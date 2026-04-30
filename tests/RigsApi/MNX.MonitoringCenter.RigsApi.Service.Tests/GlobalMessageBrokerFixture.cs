namespace MNX.MonitoringCenter.RigsApi.Service.Tests;

[SetUpFixture]
internal class GlobalMessageBrokerFixture
{
    public static IMessageBrokerFixture Broker { get; private set; }

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        Broker = new RabbitFixture();
        await Broker.StartAsync();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await Broker.DisposeAsync();
    }
}
