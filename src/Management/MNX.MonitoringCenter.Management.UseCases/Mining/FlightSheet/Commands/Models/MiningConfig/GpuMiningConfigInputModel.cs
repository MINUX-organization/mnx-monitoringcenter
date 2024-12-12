using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.Models.MiningConfig;

/// <summary>
/// Входная модель майнинг конфига для видеокарты.
/// </summary>
public class GpuMiningConfigInputModel : MiningConfigInputModel
{
    /// <inheritdoc/>
    public override MiningDeviceType DeviceType { get => MiningDeviceType.GPU; }
}
