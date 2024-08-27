namespace MNX.MonitoringCenter.Inventory.Contracts.Motherboard;

/// <summary>
/// Информация о поддерживаемой оперативной памяти.
/// </summary>
public sealed record MotherboardPci
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Bus.
    /// </summary>
    public int Bus { get; set; }

    /// <summary>
    /// Подключено ли устройство.
    /// </summary>
    public bool IsInstalled { get; set; }
}
