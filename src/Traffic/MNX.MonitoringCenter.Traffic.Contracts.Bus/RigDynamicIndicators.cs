using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Abstractions;

namespace MNX.MonitoringCenter.Traffic.Contracts.Bus;

/// <summary>
/// Динамические показатели ригов.
/// </summary>
public class RigDynamicIndicators
{
    /// <summary>
    /// Уникальный идентификатор рига.
    /// </summary>
    public Guid RigId { get; set; }

    /// <summary>
    /// Уникальный идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Дата и время отправки.
    /// </summary>
    public DateTime SendingDateTime { get; set; } = DateTime.Now;

    /// <summary>
    /// Время работы рига с момента последнего включения.
    /// </summary>
    public TimeOnly BootedUpTime { get; set; }

    /// <summary>
    /// Динамические показатели устройств.
    /// </summary>
    public List<IDeviceDynamicIndicators> Devices { get; set; } = new(0);
}