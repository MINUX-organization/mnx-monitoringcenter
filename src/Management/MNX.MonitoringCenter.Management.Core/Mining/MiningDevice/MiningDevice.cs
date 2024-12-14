using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;
using MNX.MonitoringCenter.Management.Core.Overclocking;

namespace MNX.MonitoringCenter.Management.Core.Mining.MiningDevice;

/// <summary>
/// Майнинг устройство.
/// </summary>
public class MiningDevice
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
    /// Идентификатор разгона.
    /// </summary>
    public Guid OverclockingId { get; set; }

    /// <summary>
    /// Разгон.
    /// </summary>
    public IOverclocking? Overclocking { get; private set; }

    /// <summary>
    /// Задать разгон.
    /// </summary>
    /// <param name="overclocking"> Разгон. </param>
    public void SetOverclocking(IOverclocking overclocking)
    {
        OverclockingId = overclocking.Id;
        Overclocking = overclocking;
    }

    /// <summary>
    /// Получение хеш кода майнинга устройства.
    /// </summary>
    /// <returns> Хеш код. </returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name, Type);
    }
}
