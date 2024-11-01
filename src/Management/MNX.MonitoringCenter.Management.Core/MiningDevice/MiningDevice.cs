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
    public Guid RigId { get; init; }

    /// <summary>
    /// Тип майнинг устройства.
    /// </summary>
    public MiningDeviceType Type { get; init; }
}
