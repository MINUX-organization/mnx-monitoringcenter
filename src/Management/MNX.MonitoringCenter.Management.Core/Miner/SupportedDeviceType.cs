using MNX.MonitoringCenter.Management.Core.Enums;

namespace MNX.MonitoringCenter.Management.Core.Miner;

/// <summary>
/// Поддерживаемое устройство майнера. 
/// </summary>
public class SupportedDeviceType
{
    /// <summary>
    /// Идентификатор майнера.
    /// </summary>
    public Guid MinerId { get; set; }

    /// <summary>
    /// Тип дивайса.
    /// </summary>
    public SupportedDeviceEnum DeviceType { get; set; }
}