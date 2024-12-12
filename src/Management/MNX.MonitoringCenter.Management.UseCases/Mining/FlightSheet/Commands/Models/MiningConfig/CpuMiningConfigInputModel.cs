using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.Models.MiningConfig;

/// <summary>
/// Входная модель майнинг конфига для процессора.
/// </summary>
public class CpuMiningConfigInputModel : MiningConfigInputModel
{
    /// <inheritdoc/>
    public override MiningDeviceType DeviceType { get => MiningDeviceType.CPU; }

    /// <summary>
    /// Страницы.
    /// </summary>
    public int? HugePages { get; init; }

    /// <summary>
    /// Кол-во потоков.
    /// </summary>
    public int? ThreadsCount { get; init; }
}
