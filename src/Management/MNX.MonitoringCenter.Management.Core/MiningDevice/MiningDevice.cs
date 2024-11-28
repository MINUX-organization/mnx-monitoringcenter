using MNX.MonitoringCenter.Management.Core.MiningDevice.Enums;

namespace MNX.MonitoringCenter.Management.Core.MiningDevice;

/// <summary>
/// Майнинг устройство.
/// </summary>
public class MiningDevice
{
    /// <summary>
    /// Идентификатор майнинг устройства.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Идентификатор рига.
    /// </summary>
    public Guid RigId { get; set; }

    /// <summary>
    /// Идентификатор владельца.
    /// </summary>
    public Guid OwnerId { get; set; }

    /// <summary>
    /// Тип майнинг устройства.
    /// </summary>
    public MiningDeviceType Type { get; init; }

    /// <summary>
    /// Получение хеш кода майнинга устройства.
    /// </summary>
    /// <returns> Хеш код. </returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Type);
    }
}
