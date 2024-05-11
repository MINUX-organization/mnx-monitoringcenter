using MNX.MonitoringCenter.Monitoring.Core.Devices.Enums;

namespace MNX.MonitoringCenter.Monitoring.Core.Devices;

/// <summary>
/// Жёсткий диск.
/// </summary>
public class Hdd : MiningDevice
{
    /// <inheritdoc/>
    public override MiningDeviceType Type
    {
        get => MiningDeviceType.HDD;
    }

    /// <summary>
    /// Вместимость.
    /// </summary>
    public int Capacity { get; }

    public Hdd(Guid id, string name, string rigName, string manufacturer, string serialNumber, int capacity)
        : base(id, name, rigName, manufacturer, serialNumber)
    {
        Capacity = capacity;
    }
}
