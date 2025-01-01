using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Mining;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices;
using MNX.MonitoringCenter.RigsApi.Contracts.FlightSheet;

namespace MNX.MonitoringCenter.RigsApi.Contracts.Device.Abstractions;

/// <summary>
/// Динамические параметры майнинга устройства.
/// </summary>
public abstract class DeviceDynamicMiningIndicatorsModel
{
    public Guid DeviceId { get; init; }

    public string? DeviceName { get; init; }
    
    public abstract DeviceType Type { get; }
    
    public FlightSheetStatisticsModel? FlightSheet { get; init; }
    
    public MiningState MiningState { get; init; }
}
