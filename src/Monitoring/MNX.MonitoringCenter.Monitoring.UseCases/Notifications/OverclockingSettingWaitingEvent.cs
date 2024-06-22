using MediatR;
using MNX.MonitoringCenter.Monitoring.Contracts.Bus.Models;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Notifications;

/// <summary>
/// Событие об ожидании установки разгона для видеокарты.
/// </summary>
public class OverclockingSettingWaitingEvent : INotification
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
    public OverclockingModel Overclocking { get; set; }

    public OverclockingSettingWaitingEvent(long userId, string connectionid, Guid cardId, OverclockingModel overclocking)
    {
        UserId = userId;
        ConnectionId = connectionid;
        CardId = cardId;
        Overclocking = overclocking;
    }
}
