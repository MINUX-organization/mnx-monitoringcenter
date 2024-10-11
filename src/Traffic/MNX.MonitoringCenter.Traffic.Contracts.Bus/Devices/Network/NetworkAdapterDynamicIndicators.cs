using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Abstractions;

namespace MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Network;

/// <summary>
/// Динамические показатели сетевого адаптера.
/// </summary>
public class NetworkAdapterDynamicIndicators : DeviceDynamicIndicators
{
    /// <inheritdoc/>
    public override DeviceType Type { get => DeviceType.NetworkAdapter; }

    /// <summary>
    /// Признак того, что адаптер используется.
    /// </summary>
    public bool IsUse { get; init; }

    /// <summary>
    /// Уровень интернет соединения.
    /// </summary>
    public OnlineState OnlineState { get; init; }

    /// <summary>
    /// Скорость интернета.
    /// </summary>
    public int InternetSpeed { get; init; }
}
