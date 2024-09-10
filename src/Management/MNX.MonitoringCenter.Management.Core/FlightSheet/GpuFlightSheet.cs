namespace MNX.MonitoringCenter.Management.Core.FlightSheet;

/// <summary>
/// Полётный лист видеокарты.
/// </summary>
public class GpuFlightSheet : FlightSheetBase, IEquatable<GpuFlightSheet>
{
    /// <summary>
    /// Тип.
    /// </summary>
    private FlightSheetType _type;

    /// <inheritdoc/>
    public override FlightSheetType Type 
    { 
        get => _type; 
        init => _type = FlightSheetType.GPU; 
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is GpuFlightSheet flightSheet)
        {
            return Equals(flightSheet);
        }

        return false;
    }

    /// <inheritdoc/>
    public bool Equals(GpuFlightSheet? other)
    {
        return base.Equals(other);
    }
}