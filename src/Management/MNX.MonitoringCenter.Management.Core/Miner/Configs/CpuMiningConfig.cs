using MNX.MonitoringCenter.Management.Core.MiningDevice.Enums;

namespace MNX.MonitoringCenter.Management.Core.Miner.Configs;

/// <summary>
/// Конфиг для майнинга на процессоре.
/// </summary>
public class CpuMiningConfig : BaseMiningConfig, IEquatable<CpuMiningConfig>
{
    /// <inheritdoc/>
    public override MiningDeviceType DeviceType { get => MiningDeviceType.CPU; }

    /// <summary>
    /// Страницы.
    /// </summary>
    public int? HugePages { get; set; }

    /// <summary>
    /// Кол-во потоков.
    /// </summary>
    public int? ThreadsCount { get; set; }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is CpuMiningConfig flightSheet)
        {
            return Equals(flightSheet);
        }

        return false;
    }

    /// <inheritdoc/>
    public bool Equals(CpuMiningConfig? other)
    {
        return base.Equals(other) && HugePages == other.HugePages;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), HugePages);
    }
}
