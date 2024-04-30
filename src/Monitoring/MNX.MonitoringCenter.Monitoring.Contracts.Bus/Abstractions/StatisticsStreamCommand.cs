using MNX.MonitoringCenter.Monitoring.Contracts.Bus.Enums;

namespace MNX.MonitoringCenter.Monitoring.Contracts.Bus.Abstractions;

/// <summary>
/// Команда потока статистики.
/// </summary>
public abstract class StatisticsStreamCommand
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// Тип наблюдаемых объектов.
    /// </summary>
    public ObservableObjectsType ObservableObjectsType { get; set; }
}
