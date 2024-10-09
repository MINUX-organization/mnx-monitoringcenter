using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices;
using MNX.MonitoringCenter.Traffic.Observers.Hardware.Contracts.Devices.Abstractions;

namespace MNX.MonitoringCenter.Traffic.Observers.Hardware.Contracts.Devices;

/// <summary>
/// Динамические аппаратные показатели видеокарты.
/// </summary>
public class GpuDynamicHardwareIndicators : DeviceDynamicHardwareIndicators
{
    /// <inheritdoc/>
    public override DeviceType Type { get; } = DeviceType.GPU;

    /// <summary>
    /// Температура памяти.
    /// </summary>
    public int MemoryTemperature { get; init; }

    /// <summary>
    /// Температура ядра.
    /// </summary>
    public int CoreTemperature { get; init; }

    /// <summary>
    /// Скорость вентилятора.
    /// </summary>
    public int FanSpeed { get; init; }

    /// <summary>
    /// Получить среднюю температуру.
    /// </summary>
    /// <returns> Средняя температура. </returns>
    public int GetAverageTemperature()
    {
        return ( MemoryTemperature + CoreTemperature ) / 2;
    }
}
