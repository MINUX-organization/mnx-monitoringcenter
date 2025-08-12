using MNX.MonitoringCenter.Management.Core.Overclocking.Enums;

namespace MNX.MonitoringCenter.Management.Core.Overclocking.Gpu.Fan;

/// <summary>
/// Разгон скорости вентилятора, задаваемый целевой температурой.
/// </summary>
public class FanOverclockingWithTargetTemperature : IFanOverclocking, IEquatable<FanOverclockingWithTargetTemperature>
{
    /// <inheritdoc/>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <inheritdoc/>
    public FanOverclockingType Type
    {
        get => FanOverclockingType.TargetTemperature;
    }

    /// <summary>
    /// Минимальная целевая скорость вентилятора (в процентах %).
    /// </summary>
    public int MinTargetSpeed { get; set; }

    /// <summary>
    /// Максимальная целевая скорость вентилятора (в процентах %).
    /// </summary>
    public int MaxTargetSpeed { get; set; }

    /// <summary>
    /// Целевое значение температуры процессора (в градусах по Цельсию °C).
    /// </summary>
    public int TargetCoreTemperature { get; set; }

    /// <summary>
    /// Целевое значение температуры памяти (в градусах по Цельсию °C).
    /// </summary>
    public int TargetMemoryTemperature { get; set; }

    /// <inheritdoc/>
    public object Clone()
    {
        return new FanOverclockingWithTargetTemperature()
        {
            Id = Id,
            MinTargetSpeed = MinTargetSpeed,
            MaxTargetSpeed = MaxTargetSpeed,
            TargetCoreTemperature = TargetCoreTemperature,
            TargetMemoryTemperature = TargetMemoryTemperature
        };
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is FanOverclockingWithTargetTemperature fanOverclocking)
        {
            return Equals(fanOverclocking);
        }

        return false;
    }

    /// <inheritdoc/>
    public bool Equals(FanOverclockingWithTargetTemperature? other)
    {
        if (other is null) return false;

        if (ReferenceEquals(other, null)) return true;

        return MinTargetSpeed == other.MinTargetSpeed &&
               MaxTargetSpeed == other.MaxTargetSpeed &&
               TargetCoreTemperature == other.TargetCoreTemperature &&
               TargetMemoryTemperature == other.TargetMemoryTemperature;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        HashCode hash = new();

        hash.Add(MinTargetSpeed);
        hash.Add(MaxTargetSpeed);
        hash.Add(TargetCoreTemperature);
        hash.Add(TargetMemoryTemperature);

        return hash.ToHashCode();
    }
}
