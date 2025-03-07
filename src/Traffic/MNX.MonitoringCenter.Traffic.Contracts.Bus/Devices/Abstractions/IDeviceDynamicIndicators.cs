using MessagePack;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Mining;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Network;

namespace MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Abstractions;

/// <summary>
/// Динамические показатели устройства.
/// </summary>
[Union(0, typeof(CpuDynamicIndicators))]
[Union(1, typeof(GpuDynamicIndicators))]
[Union(2, typeof(NetworkAdapterDynamicIndicators))]
public interface IDeviceDynamicIndicators
{
    /// <summary>
    /// Идентификатор устройства.
    /// </summary>
    Guid DeviceId { get; set; }

    /// <summary>
    /// Тип устройства.
    /// </summary>
    DeviceType Type { get; }

    /// <summary>
    /// Мощность.
    /// </summary>
    int Power { get; set; }
}
