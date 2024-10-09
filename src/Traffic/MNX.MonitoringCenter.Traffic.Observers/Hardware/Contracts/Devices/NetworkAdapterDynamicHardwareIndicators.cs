using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Network;
using MNX.MonitoringCenter.Traffic.Observers.Hardware.Contracts.Devices.Abstractions;

namespace MNX.MonitoringCenter.Traffic.Observers.Hardware.Contracts.Devices;

/// <summary>
/// Динамические аппаратные показатели сетевого адаптера.
/// </summary>
public class NetworkAdapterDynamicHardwareIndicators : NetworkAdapterDynamicIndicators, IDeviceDynamicHardwareIndicators { }
