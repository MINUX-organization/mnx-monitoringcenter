using MNX.MonitoringCenter.Monitoring.Service.Messages.Models;

namespace MNX.MonitoringCenter.Monitoring.Service.Messages;

/// <summary>
/// Сообщение об изменении обобщённых данных.
/// </summary>
public class TotalDataChangeMessage
{
    /// <summary>
    /// Тип объектов, для которых высчитываются обобщённые данные.
    /// </summary>
    public TotalDataType Type { get; set; }

    /// <summary>
    /// Новое значение.
    /// </summary>
    public object NewData { get; set; }
}
