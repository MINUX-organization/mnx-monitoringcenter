namespace MNX.MonitoringCenter.Management.Core.FlightSheet.Target;

/// <summary>
/// Таргет полётного листа для видеокарты.
/// </summary>
public class GpuFlightSheetTarget : FlightSheetTargetBase, IEquatable<GpuFlightSheetTarget>
{
    /// <summary>
    /// Тип.
    /// </summary>
    private FlightSheetTargetType _type;

    /// <inheritdoc/>
    public override FlightSheetTargetType Type
    {
        get => _type;
        init => _type = FlightSheetTargetType.GPU;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is GpuFlightSheetTarget flightSheet)
        {
            return Equals(flightSheet);
        }

        return false;
    }

    /// <inheritdoc/>
    public bool Equals(GpuFlightSheetTarget? other)
    {
        return base.Equals(other);
    }
}