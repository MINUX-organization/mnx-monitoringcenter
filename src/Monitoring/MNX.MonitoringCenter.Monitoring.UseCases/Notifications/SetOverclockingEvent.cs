using MediatR;
using MNX.MonitoringCenter.Monitoring.Core;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Notifications;

/// <summary>
/// Событие об установке разгона для видеокарты.
/// </summary>
public class SetOverclockingEvent : INotification
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// Идентификатор соединения веб-клиента.
    /// </summary>
    public string ConnectionId { get; set; }

    /// <summary>
    /// Идентификатор видеокарты.
    /// </summary>
    public Guid CardId { get; set; }

    /// <summary>
    /// Разгон видеокарты.
    /// </summary>
    public Overclocking Overclocking { get; set; }

    public SetOverclockingEvent(long userId, string connectionid, Guid cardId, Overclocking overclocking)
    {
        UserId = userId;
        ConnectionId = connectionid;
        CardId = cardId;
        Overclocking = overclocking;
    }
}
