using MNX.MonitoringCenter.Management.Core.Overclocking.Enums;

namespace MNX.MonitoringCenter.Management.Core.Overclocking.Gpu.Fan;

/// <summary>
/// Реализация <see cref="IFanOverclocking"/>.
/// Разгон вентилятора видеокарты по целевому значению скорости вентилятора.
/// </summary>
public class FanOverclockingWithTargetSpeed : IFanOverclocking, IEquatable<FanOverclockingWithTargetSpeed>
{
    /// <inheritdoc/>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <inheritdoc/>
    public FanOverclockingType Type
    {
        get => FanOverclockingType.TargetSpeed;
    }

    /// <summary>
    /// Целевое значение разгона вентилятора (в процентах %).
    /// </summary>
    public int TargetSpeed { get; set; }

    /// <inheritdoc/>
    public object Clone()
    {
        return new FanOverclockingWithTargetSpeed()
        {
            Id = Id,
            TargetSpeed = TargetSpeed
        };
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is FanOverclockingWithTargetSpeed fanOverclocking)
        {
            return Equals(fanOverclocking);
        }

        return false;
    }

    /// <inheritdoc/>
    public bool Equals(FanOverclockingWithTargetSpeed? other)
    {
        if (other is null) return false;

        if (ReferenceEquals(this, other)) return true;

        return TargetSpeed == other.TargetSpeed;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        HashCode hash = new();

        hash.Add(TargetSpeed);

        return hash.ToHashCode();
    }
}
