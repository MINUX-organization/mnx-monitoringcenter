namespace MNX.MonitoringCenter.Management.Core.FlightSheet;

/// <summary>
/// Полётный лист процессора.
/// </summary>
public class CpuFlightSheet : FlightSheetBase, IEquatable<CpuFlightSheet>
{
    /// <summary>
    /// Тип.
    /// </summary>
    private FlightSheetType _type;

    /// <inheritdoc/>
    public override FlightSheetType Type 
    { 
        get => _type; 
        init => _type = FlightSheetType.CPU; 
    }

    /// <summary>
    /// Страницы.
    /// </summary>
    public int HugePage { get; set; }

    /// <summary>
    /// Строка конфигурации формата Json.
    /// </summary>
    public string? ConfigFile { get; set; }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is CpuFlightSheet flightSheet)
        {
            return Equals(flightSheet);
        }

        return false;
    }

    /// <inheritdoc/>
    public bool Equals(CpuFlightSheet? other)
    {
        return base.Equals(other) && HugePage == other.HugePage && ConfigFile == other.ConfigFile;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), HugePage, ConfigFile);
    }
}