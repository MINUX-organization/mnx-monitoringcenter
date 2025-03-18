using MNX.MonitoringCenter.Management.Contracts.MiningDevice;
using MNX.MonitoringCenter.Management.Core.Mining.FlightSheet.Target;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet;

/// <summary>
/// Полётный лист устройства.
/// </summary>
public class DeviceFLightSheet
{
    /// <summary>
    /// Майнинга устройство.
    /// </summary>
    public MiningDeviceModel Device { get; }

    /// <summary>
    /// Таргет полётного листа.
    /// </summary>
    public FlightSheetTarget? FlightSheet { get; }

    public DeviceFLightSheet(MiningDeviceModel device, Core.Mining.FlightSheet.FlightSheet? flightSheet = null)
    {
        Device = device;
        FlightSheet = flightSheet?.Targets.First(x => x.DeviceType.ToString() == device.Type);
    }
}
