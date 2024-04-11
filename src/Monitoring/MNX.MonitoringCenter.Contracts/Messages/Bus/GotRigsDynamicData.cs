using MNX.MonitoringCenter.Monitoring.Contracts.Models;

namespace MNX.MonitoringCenter.Monitoring.Contracts.Messages.Bus;

/// <summary>
/// Получены динамические данные с ригов от агрегатора
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
