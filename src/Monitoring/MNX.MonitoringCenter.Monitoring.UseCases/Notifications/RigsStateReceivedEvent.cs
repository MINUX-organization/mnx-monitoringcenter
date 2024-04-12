using MediatR;
using MNX.MonitoringCenter.Monitoring.Contracts.Models;
using MNX.MonitoringCenter.Monitoring.UseCases.Queries;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Notifications;

/// <summary>
/// Событие о получении состояния ригов.
/// </summary>
public class RigsStateReceivedEvent : INotification
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
    /// Состояние ригов.
    /// </summary>
    public List<RigState> RigsState { get; }

    public RigsStateReceivedEvent(string subscriberId, Specification specification, List<RigState> rigsState)
    {
        SubscriberId = subscriberId;
        Specification = specification;
        RigsState = rigsState;
    }
}
