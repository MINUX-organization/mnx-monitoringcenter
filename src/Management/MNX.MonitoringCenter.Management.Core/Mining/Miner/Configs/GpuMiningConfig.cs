using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;

namespace MNX.MonitoringCenter.Management.Core.Mining.Miner.Configs;

/// <summary>
/// Конфиг для майнинга на видеокарте.
/// </summary>
public class GpuMiningConfig : BaseMiningConfig, IEquatable<GpuMiningConfig>
{
    /// <inheritdoc/>
    public override MiningDeviceType DeviceType { get => MiningDeviceType.GPU; }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is GpuMiningConfig config)
        {
            return Equals(config);
        }

        return false;
    }

    /// <inheritdoc/>
    public bool Equals(GpuMiningConfig? other)
    {
        return base.Equals(other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
