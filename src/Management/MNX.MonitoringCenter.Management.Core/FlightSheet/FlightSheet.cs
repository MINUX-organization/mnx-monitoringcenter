using MNX.MonitoringCenter.Management.Core.FlightSheet.Target;

namespace MNX.MonitoringCenter.Management.Core.FlightSheet;

/// <summary>
/// Полётный лист.
/// </summary>
public class FlightSheet : IEquatable<FlightSheet>
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Название.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Таргеты.
    /// </summary>
    public List<FlightSheetTargetBase> Targets { get; set; } = new(2);

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is FlightSheet flightSheet)
        {
            return Equals(flightSheet);
        }

        return false;
    }

    /// <inheritdoc/>
    public bool Equals(FlightSheet? other)
    {
        if (other is null) return false;

        if (ReferenceEquals(this, other)) return true;

        return Name == other.Name && UserId == other.UserId && Targets.SequenceEqual(other.Targets);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        int targetsHash = 0;

        foreach (var target in Targets)
        {
            targetsHash += target.GetHashCode();
        }

        return HashCode.Combine(Name, UserId, targetsHash);
    }
}
