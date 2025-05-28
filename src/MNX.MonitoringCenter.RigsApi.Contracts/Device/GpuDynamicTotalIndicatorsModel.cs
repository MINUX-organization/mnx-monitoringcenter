using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices;
using MNX.MonitoringCenter.RigsApi.Contracts.Device.Abstractions;

namespace MNX.MonitoringCenter.RigsApi.Contracts.Device;

/// <summary>
/// Динамические общие показатели видеокарты.
/// </summary>
public class GpuDynamicTotalIndicatorsModel : DeviceDynamicTotalIndicators
{
    /// <inheritdoc/>
    public override DeviceType Type => DeviceType.GPU;

    /// <summary>
    /// Скорость вентилятора.
    /// </summary>
    public int FanSpeed { get; init; }

    /// <summary>
    /// Температура памяти.
    /// </summary>
    public int MemoryTemperature { get; init; }

    /// <summary>
    /// Температура ядра.
    /// </summary>
    public int CoreTemperature { get; init; }

    /// <summary>
    /// Средняя температура.
    /// </summary>
    public int AvarageTemperature { get; init; }
}
