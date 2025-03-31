namespace MNX.MonitoringCenter.Management.Core.Overclocking;

/// <summary>
/// Пресет.
/// </summary>
public class Preset : IEquatable<Preset>
{
    /// <summary>
    /// Уникальный идентификатор.
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Название пресета.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Название майнинг устройства.
    /// </summary>
    public required string DeviceName { get; set; }

    /// <summary>
    /// Идентификатор разгона.
    /// </summary>
    public Guid OverclockingId { get; set; }

    /// <summary>
    /// Модель с разгоном
    /// </summary>
    public IOverclocking? Overclocking { get; set; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Признак видимости пресета.
    /// </summary>
    /// <remarks>
    /// Если false, то данный пресет существует,
    /// как невидимый для пользователя пресет.
    /// Со стороны пользователя происходит работа
    /// разгона напрямую с майнинг-девайсом.
    /// В противном случае подразумевается, что
    /// пользователь работает с разгоном через пресет.
    /// </remarks>
    public bool IsVisible { get; set; } = false;

    /// <summary>
    /// Получить признак поддержки устройства.
    /// </summary>
    /// <param name="deviceName">
    /// Полное наименование устройства.
    /// </param>
    /// <returns>
    /// Признак поддержки устройства.
    /// </returns>
    public bool IsDeviceSupport(string deviceName)
    {
        return DeviceName == deviceName;
    }

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
               DeviceName == other.DeviceName &&
              (Overclocking?.Equals(other.Overclocking) ?? other.Overclocking is null);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(Name, DeviceName, Overclocking);
    }
}