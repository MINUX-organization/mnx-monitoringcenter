using MediatR;
using MNX.MonitoringCenter.Monitoring.Contracts.Bus.Models;
using MNX.MonitoringCenter.Monitoring.UseCases.Commands.ComputeTotalRigsDynamicData.Models;
using MNX.MonitoringCenter.Monitoring.UseCases.Queries;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Notifications.UpdateDynamicData;

/// <summary>
/// Событие об обновлении у подписчика динамических данных ригов.
/// </summary>
public class SubscriberRigsDynamicDataUpdateEvent : INotification
{
    /// <summary>
    /// Идентификатор подписчика.
    /// </summary>
    public string SubscriberId { get; }

    /// <summary>
    /// Спецификация.
    /// </summary>
    public Specification Specification { get; }

    /// <summary>
    /// Динамические данные ригов.
    /// </summary>
    public List<RigDynamicData> RigsDynamicData { get; }

    /// <summary>
    /// Скорость хеширования по отслеживаемой монете.
    /// </summary>
    public HashRateModel? HashRateByObservableCoin { get; }

    public SubscriberRigsDynamicDataUpdateEvent(string subscriberId,
                                                Specification specification,
                                                List<RigDynamicData> rigsDynamicData,
                                                HashRateModel? hashRateByObservableCoin = null)
    {
        SubscriberId = subscriberId;
        Specification = specification;
        RigsDynamicData = rigsDynamicData;
        HashRateByObservableCoin = hashRateByObservableCoin;
    }
}
