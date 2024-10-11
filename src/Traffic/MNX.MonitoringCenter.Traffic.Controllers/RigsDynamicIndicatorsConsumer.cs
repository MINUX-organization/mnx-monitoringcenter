using EasyNetQ.AutoSubscribe;
using MNX.MonitoringCenter.Traffic.Contracts.Bus;
using MNX.MonitoringCenter.Traffic.Observers.Abstractions;

namespace MNX.MonitoringCenter.Traffic.Controllers;

/// <summary>
/// Потребитель динамических показателей ригов.
/// </summary>
public class RigsDynamicIndicatorsConsumer : IConsume<RigDynamicIndicators>
{
    private readonly IUserRigsObserverAggregator _userRigsObserverAggregator;

    public RigsDynamicIndicatorsConsumer(IUserRigsObserverAggregator userRigsObserverAggregator)
    {
        _userRigsObserverAggregator = userRigsObserverAggregator
            ?? throw new ArgumentNullException(nameof(userRigsObserverAggregator));
    }

    public void Consume(RigDynamicIndicators message, CancellationToken cancellationToken = default)
    {
        _userRigsObserverAggregator.SetIndicators(message.UserId, message);
    }
}
