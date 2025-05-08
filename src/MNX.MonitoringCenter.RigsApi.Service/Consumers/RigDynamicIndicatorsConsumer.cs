using EasyNetQ;
using EasyNetQ.Topology;
using MNX.MonitoringCenter.Traffic.Contracts.Bus;
using MNX.MonitoringCenter.Traffic.Observers.Abstractions;

namespace MNX.MonitoringCenter.RigsApi.Service.Consumers;

/// <summary>
/// Потребитель динамических показателей рига.
/// </summary>
public class RigDynamicIndicatorsConsumer : IHostedService
{
    private const string EXCHANGE_NAME =
        "MNX.MonitoringCenter.Traffic.Contracts.Bus.RigDynamicIndicators, MNX.MonitoringCenter.Traffic.Contracts.Bus";

    private const string QUEUE_NAME =
        "MNX.MonitoringCenter.Traffic.Contracts.Bus.RigDynamicIndicators, MNX.MonitoringCenter.Traffic.Contracts.Bus_RigDynamicIndicators, rigs_api";

    private IDisposable? _subscription;

    private readonly IAdvancedBus _bus;

    private readonly IUserRigsObserverAggregator _userRigsObserverAggregator;

    public RigDynamicIndicatorsConsumer(IAdvancedBus bus,
                                        IUserRigsObserverAggregator userRigsObserverAggregator)
    {
        _bus = bus ?? throw new ArgumentNullException(nameof(bus));

        _userRigsObserverAggregator = userRigsObserverAggregator
            ?? throw new ArgumentNullException(nameof(userRigsObserverAggregator));
    }

    /// <inheritdoc/>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        var exchange = _bus.ExchangeDeclare(
            name: EXCHANGE_NAME,
            type: ExchangeType.Fanout,
            cancellationToken: cancellationToken
        );

        var queue = _bus.QueueDeclare(QUEUE_NAME, configs =>
        {
            configs.AsAutoDelete(true);
            configs.WithMaxLength(10_000);
        },
        cancellationToken);

        _bus.Bind(exchange, queue, string.Empty, cancellationToken);

        _subscription = _bus.Consume<RigDynamicIndicators>(queue, ProcessMessage, configs =>
        {
            configs.WithPrefetchCount(10_000);
        });

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_subscription is IAsyncDisposable asyncDisposable)
        {
            await asyncDisposable.DisposeAsync();
        }
        else
        {
            _subscription?.Dispose();
        }
        
        _subscription = null;
    }

    private void ProcessMessage(IMessage<RigDynamicIndicators> message, MessageReceivedInfo info)
    {
        if (message.GetBody() is RigDynamicIndicators indicators)
        {
            _userRigsObserverAggregator.SetIndicators(indicators.UserId, indicators);
        }
    }
}
