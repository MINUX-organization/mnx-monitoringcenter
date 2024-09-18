namespace MNX.MonitoringCenter.Management.Core.FlightSheet.Target;

/// <summary>
/// Таргет полётного листа.
/// </summary>
public abstract class FlightSheetTargetBase : IEquatable<FlightSheetTargetBase>
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Тип.
    /// </summary>
    public abstract FlightSheetTargetType Type { get; init; }

    /// <summary>
    /// Идентификатор полётного листа.
    /// </summary>
    public Guid FlightSheetId { get; set; }

    /// <summary>
    /// Список конфигов для майнинга.
    /// </summary>
    public List<FlightSheetTargetConfig> Configs { get; set; } = new(3);

    /// <summary>
    /// Строка аргументов для майнера.
    /// </summary>
    public string? AdditionalArguments { get; set; }

    /// <summary>
    /// Название майнера.
    /// </summary>
    public Guid MinerId { get; set; }

    /// <summary>
    /// Майнер.
    /// </summary>
    public Miner? Miner { get; set; }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is FlightSheetTargetBase flightSheet)
        {
            return Equals(flightSheet);
        }

        return false;
    }

    /// <inheritdoc/>
    public virtual bool Equals(FlightSheetTargetBase? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Type == other.Type && MinerId == other.MinerId &&
               AdditionalArguments == other.AdditionalArguments &&
               Configs.SequenceEqual(other.Configs);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        var hashCode = 0;

        foreach (var config in Configs)
        {
            hashCode += config.GetHashCode();
        }

        return HashCode.Combine(hashCode, Type, AdditionalArguments, MinerId);
    }
}
