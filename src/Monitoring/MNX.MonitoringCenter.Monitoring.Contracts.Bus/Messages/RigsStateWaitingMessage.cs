namespace MNX.MonitoringCenter.Monitoring.Contracts.Bus.Messages;

/// <summary>
/// Сообщение об ожидании состояния ригов.
/// </summary>
public class RigsStateWaitingMessage
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Идентификатор соединения веб-клиента.
    /// </summary>
    public string ConnectionId { get; set; }
}