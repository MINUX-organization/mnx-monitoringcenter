using MNX.MonitoringCenter.Management.Core.Overclocking.Enums;

namespace MNX.MonitoringCenter.Management.Core.Overclocking.Gpu;

/// <summary>
/// Разгон разгона видеокарт модели Intel.
/// </summary>
public class IntelGpuOverclocking : IOverclocking, IEquatable<IntelGpuOverclocking>
{
    /// <inheritdoc/>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <inheritdoc/>
    public OverclockingTargetDeviceType TargetDeviceType
    {
        get => OverclockingTargetDeviceType.IntelGPU;
    }

    /// <inheritdoc/>
    public object Clone()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public bool Equals(IntelGpuOverclocking? other)
    {
        throw new NotImplementedException();
    }
}
