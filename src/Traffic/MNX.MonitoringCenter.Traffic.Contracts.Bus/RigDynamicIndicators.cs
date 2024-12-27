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
    public Guid RigId { get; init; }

    /// <summary>
    /// Уникальный идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Дата и время отправки.
    /// </summary>
    public DateTime SendingDateTime { get; init; } = DateTime.Now;

    /// <summary>
    /// Время работы рига с момента последнего включения.
    /// </summary>
    public DateTime BootedUpTime { get; init; }

    /// <summary>
    /// Время майнинга с момента последнего включения.
    /// </summary>
    public DateTime MiningUpTime { get; init; }

    /// <summary>
    /// Динамические показатели устройств.
    /// </summary>
    public List<IDeviceDynamicIndicators> Devices { get; init; } = new(0);
}