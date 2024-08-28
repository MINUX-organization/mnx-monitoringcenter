namespace MNX.MonitoringCenter.Inventory.Contracts.Motherboard;

/// <summary>
/// Информация о поддерживаемой оперативной памяти.
/// </summary>
public record MotherboardPci
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Bus.
    /// </summary>
    public int Bus { get; init; }

    /// <summary>
    /// Подключено ли устройство.
    /// </summary>
    public bool IsInstalled { get; init; }
}
