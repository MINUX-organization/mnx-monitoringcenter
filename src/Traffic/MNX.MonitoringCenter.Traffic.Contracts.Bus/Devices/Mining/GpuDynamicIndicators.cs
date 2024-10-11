namespace MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Mining;

/// <summary>
/// Динамические показатели видеокарты.
/// </summary>
public class GpuDynamicIndicators : MiningDeviceDynamicIndicators
{
    /// <inheritdoc/>
    public override DeviceType Type { get => DeviceType.GPU; }

    /// <summary>
    /// Температура памяти.
    /// </summary>
    public int MemoryTemperature { get; init; }

    /// <summary>
    /// Температура ядра.
    /// </summary>
    public int CoreTemperature { get; init; }

    /// <inheritdoc/>
    public override int GetAverageTemperature()
    {
        return (MemoryTemperature + CoreTemperature) / 2;
    }
}
