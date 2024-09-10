namespace MNX.MonitoringCenter.Management.Core;

/// <summary>
/// Пресет
/// </summary>
public class Preset : IEquatable<Preset>
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Название пресета
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Название GPU
    /// </summary>
    public required string GpuName { get; set; }

    /// <summary>
    /// Идентификатор разгона.
    /// </summary>
    public Guid OverclockingId { get; set; }

    /// <summary>
    /// Модель с разгоном
    /// </summary>
    public Overclocking? Overclocking { get; set; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; init; }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is Preset preset)
        {
            return Equals(preset);
        }

        return false;
    }

    /// <inheritdoc/>
    public bool Equals(Preset? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Name == other.Name &&
               GpuName == other.GpuName &&
              (Overclocking?.Equals(other.Overclocking) ?? other.Overclocking is null);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(Name, GpuName, Overclocking);
    }
}