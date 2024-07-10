namespace MNX.MonitoringCenter.Monitoring.DataAccess.Dto.Devices;

/// <summary>
/// Жёсткий диск.
/// </summary>
public class HddDto : MiningDeviceDto
{
    /// <summary>
    /// Вместимость.
    /// </summary>
    public int Capacity { get; }
}
