using MediatR;
using MNX.MonitoringCenter.Monitoring.UseCases.Queries.GetRigsInformation;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Notifications;

/// <summary>
/// Событие о получении информации о ригах.
/// </summary>
public class GotRigsInformationEvent : INotification
{
    /// <summary>
    /// Идентификатор подписки.
    /// </summary>
    public string SubscriberId { get; }

    /// <summary>
    /// Информация о ригах.
    /// </summary>
    public GetRigsInformationResult Information { get; }

    public GotRigsInformationEvent(string subscriberId, GetRigsInformationResult information)
    {
        SubscriberId = subscriberId;
        Information = information;
    }
}
