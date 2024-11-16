using MNX.MonitoringCenter.Management.Core.MiningDevice.Enums;

namespace MNX.MonitoringCenter.Management.UseCases.FlightSheet.Commands.Models.MiningConfig;

/// <summary>
/// Входная модель майнинг конфига для видеокарты.
/// </summary>
public class GpuMiningConfigInputModel : MiningConfigInputModel
{
    /// <inheritdoc/>
    public override MiningDeviceType DeviceType { get => MiningDeviceType.GPU; }
}
