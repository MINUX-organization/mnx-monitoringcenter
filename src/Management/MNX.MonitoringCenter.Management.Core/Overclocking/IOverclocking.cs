using MNX.MonitoringCenter.Management.Core.Overclocking.Enums;

namespace MNX.MonitoringCenter.Management.Core.Overclocking;

/// <summary>
/// Разгон.
/// </summary>
public interface IOverclocking : ICloneable
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Тип целевого устройства.
    /// </summary>
    public OverclockingTargetDeviceType TargetDeviceType { get; }
}
