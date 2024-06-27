using MediatR;
using MNX.MonitoringCenter.Monitoring.Core;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Notifications;

/// <summary>
/// Событие об удачном применении разгона видеокарте.
/// </summary>
public class OverclockingSettingSuccessEvent : INotification
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
    /// Разгон.
    /// </summary>
    public Overclocking Overclocking { get; set; }

    public OverclockingSettingSuccessEvent(long userId, string connectionid, Guid cardId, Overclocking overclocking)
    {
        UserId = userId;
        ConnectionId = connectionid;
        CardId = cardId;
        Overclocking = overclocking;
    }
}
