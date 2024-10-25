namespace MNX.MonitoringCenter.Management.Core.FlightSheet.Target;

/// <summary>
/// Таргет полётный листа для процессора.
/// </summary>
public class CpuFlightSheetTarget : FlightSheetTargetBase, IEquatable<CpuFlightSheetTarget>
{
    /// <summary>
    /// Тип.
    /// </summary>
    private FlightSheetTargetType _type;

    /// <inheritdoc/>
    public override FlightSheetTargetType Type
    {
        get => _type;
        init => _type = FlightSheetTargetType.CPU;
    }

    /// <summary>
    /// Страницы.
    /// </summary>
    public int? HugePages { get; set; }

    /// <summary>
    /// Строка конфигурации формата Json.
    /// </summary>
    public string? ConfigFile { get; set; }

    /// <summary>
    /// Кол-во потоков.
    /// </summary>
    public int? ThreadsCount { get; set; }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is CpuFlightSheetTarget flightSheet)
        {
            return Equals(flightSheet);
        }

        return false;
    }

    /// <inheritdoc/>
    public bool Equals(CpuFlightSheetTarget? other)
    {
        return base.Equals(other) && HugePages == other.HugePages && ConfigFile == other.ConfigFile;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), HugePages, ConfigFile);
    }
}