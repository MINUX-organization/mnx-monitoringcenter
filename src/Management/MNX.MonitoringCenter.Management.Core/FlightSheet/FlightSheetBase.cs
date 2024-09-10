namespace MNX.MonitoringCenter.Management.Core.FlightSheet;

/// <summary>
/// Полётный лист.
/// </summary>
public abstract class FlightSheetBase : IEquatable<FlightSheetBase>
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Тип.
    /// </summary>
    public abstract FlightSheetType Type { get; init; }

    /// <summary>
    /// Название.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Список конфигов для майнинга.
    /// </summary>
    public List<FlightSheetConfig> Configs { get; set; } = new();

    /// <summary>
    /// Строка аргументов для майнера.
    /// </summary>
    public string? AdditionalArguments { get; set; }

    /// <summary>
    /// Название майнера.
    /// </summary>
    public string Miner { get; set; } = string.Empty;

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is FlightSheetBase flightSheet)
        {
            return Equals(flightSheet);
        }

        return false;
    }

    /// <inheritdoc/>
    public virtual bool Equals(FlightSheetBase? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Type == other.Type && Miner == other.Miner &&
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

        return HashCode.Combine(hashCode, Type, AdditionalArguments, Miner);
    }
}
