using MNX.MonitoringCenter.Monitoring.Contracts.Bus.Models;

namespace MNX.MonitoringCenter.Monitoring.Contracts.Bus.Messages;

/// <summary>
/// Сообщение об установке разгона видеокарты.
/// </summary>
public class SetOverclockingMessage
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
}
