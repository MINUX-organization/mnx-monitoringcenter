using MNX.MonitoringCenter.Monitoring.Contracts.Models;

namespace MNX.MonitoringCenter.Monitoring.Contracts.Messages.Bus;

/// <summary>
/// Получено сообщение о получении состоянии ригов от агрегатора.
/// </summary>
public class GotRigsStateMessage
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
    /// Список состояний ригов.
    /// </summary>
    public List<RigState> RigsState { get; set; } = new();
}