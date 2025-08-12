using MNX.MonitoringCenter.Management.Core.Overclocking.Enums;

namespace MNX.MonitoringCenter.Management.Core.Overclocking.Gpu.Fan;

/// <summary>
/// Разгон скорости вентилятора, задаваемый графиком.
/// </summary>
public class FanOverclockingWithLinearDependence : IFanOverclocking, IEquatable<FanOverclockingWithLinearDependence>
{
    /// <inheritdoc/>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <inheritdoc/>
    public FanOverclockingType Type
    {
        get => FanOverclockingType.LinearDependence;
    }

    /// <summary>
    /// Точки графика целевых показателей скорости вентилятора
    /// в зависимости от температуры устройства.
    /// </summary>
    public FanGraphicPoint[] TargetPoints { get; set; } = [];

    /// <inheritdoc/>
    public object Clone()
    {
        return new FanOverclockingWithLinearDependence()
        {
            Id = Id,
            TargetPoints = TargetPoints.Select(x => (FanGraphicPoint)x.Clone()).ToArray()
        };
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is FanOverclockingWithLinearDependence fanOverclocking)
        {
            return Equals(fanOverclocking);
        }

        return false;
    }

    /// <inheritdoc/>
    public bool Equals(FanOverclockingWithLinearDependence? other)
    {
        if (other is null) return false;

        if (ReferenceEquals(this, other)) return true;

        if (TargetPoints.Length != other.TargetPoints.Length) return false;

        var flag = true;
        for (int i = 0; i < TargetPoints.Length; i++)
        {
            if (TargetPoints[i].FanSpeedValueTarget != other.TargetPoints[i].FanSpeedValueTarget ||
                TargetPoints[i].TemperatureValueTarget != other.TargetPoints[i].TemperatureValueTarget)
            {
                flag = false;
            }
        }

        return flag;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        HashCode hash = new();

        foreach (var point in TargetPoints)
        {
            hash.Add(point.FanSpeedValueTarget);
            hash.Add(point.TemperatureValueTarget);
        }

        return hash.ToHashCode();
    }
}
