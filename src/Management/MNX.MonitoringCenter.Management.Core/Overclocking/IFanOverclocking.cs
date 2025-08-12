using MNX.MonitoringCenter.Management.Core.Overclocking.Enums;

namespace MNX.MonitoringCenter.Management.Core.Overclocking;

/// <summary>
/// Разгон скорости вентилятора.
/// </summary>
public interface IFanOverclocking : ICloneable
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Тип разгона вентилятора.
    /// </summary>
    public FanOverclockingType Type { get; }
}
