using MNX.MonitoringCenter.Monitoring.Contracts.Bus.Models;

namespace MNX.MonitoringCenter.Monitoring.Contracts.Bus.Messages;

/// <summary>
/// Получены динамические данные с ригов от агрегатора.
/// </summary>
public class GotRigsDynamicData
{
    /// <summary>
    /// Уникальный идентификатор пользователя.
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// Список динамических данных.
    /// </summary>
    public List<RigDynamicData> Data { get; set; } = new();
}
