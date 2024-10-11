namespace MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Mining;

/// <summary>
/// Динамические показатели процессора.
/// </summary>
public class CpuDynamicIndicators : MiningDeviceDynamicIndicators
{
    /// <inheritdoc/>
    public override DeviceType Type { get => DeviceType.CPU; }

    /// <summary>
    /// Температура.
    /// </summary>
    public int Temperature { get; init; }

    /// <inheritdoc/>
    public override int GetAverageTemperature() => Temperature;
}
