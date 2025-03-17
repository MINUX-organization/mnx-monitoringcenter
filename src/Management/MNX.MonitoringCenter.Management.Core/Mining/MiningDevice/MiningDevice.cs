using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;

namespace MNX.MonitoringCenter.Management.Core.Mining.MiningDevice;

/// <summary>
/// Майнинг устройство.
/// </summary>
public class MiningDevice : IEquatable<MiningDevice>
{
    /// <summary>
    /// Идентификатор майнинг устройства.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Идентификатор владельца.
    /// </summary>
    public Guid OwnerId { get; set; }

    /// <summary>
    /// Тип майнинг устройства.
    /// </summary>
    public MiningDeviceType Type { get; init; }

    /// <summary>
    /// Производитель.
    /// </summary>
    public required string Manufacturer { get; init; }

    /// <summary>
    /// Модель.
    /// </summary>
    public required string Model { get; init; }

    /// <summary>
    /// Название.
    /// </summary>
    public string Name { get => $"{Manufacturer} {Model}"; }

    /// <summary>
    /// Идентификатор пресета.
    /// </summary>
    public Guid PresetId { get; set; }

    /// <summary>
    /// Пресет.
    /// </summary>
    public Preset? Preset { get; set; }

    /// <summary>
    /// Задать разгон.
    /// </summary>
    /// <param name="preset"> Идентификатор пресета. </param>
    public void SetPreset(Preset preset)
    {
        PresetId = preset.Id;
        Preset = preset;
    }

    /// <summary>
    /// Получение хеш кода майнинга устройства.
    /// </summary>
    /// <returns> Хеш код. </returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name, Type);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is MiningDevice miningDevice)
        {
            return Equals(miningDevice);
        }

        return false;
    }

    /// <inheritdoc/>
    public bool Equals(MiningDevice? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Id == other.Id &&
               Name == other.Name &&
               Type == other.Type;
    }
}
