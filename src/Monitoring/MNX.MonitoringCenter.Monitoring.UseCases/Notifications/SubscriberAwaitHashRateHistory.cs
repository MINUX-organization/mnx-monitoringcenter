using MediatR;
using MNX.MonitoringCenter.Monitoring.UseCases.Commands.ComputeTotalRigsDynamicData.Models;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Notifications;

/// <summary>
/// Событие об ожидании подписчиком истории скорости хеширования.
/// </summary>
public class SubscriberAwaitHashRateHistory : INotification
{
    /// <summary>
    /// Идентификатор подписчика.
    /// </summary>
    public string SubscriberId { get; }

    /// <summary>
    /// История скорости хеширования.
    /// </summary>
    public List<HashRateModel> HashRateHistory { get; }

    public SubscriberAwaitHashRateHistory(string subscriberId, List<HashRateModel> hashRateHistory)
    {
        SubscriberId = subscriberId;
        HashRateHistory = hashRateHistory;
    }
}
