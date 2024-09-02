namespace MNX.MonitoringCenter.Inventory.Contracts.Motherboard;

/// <summary>
/// Информация о поддерживаемой оперативной памяти.
/// </summary>
public record MotherboardPci : Pci
{
    /// <summary>
    /// Подключено ли устройство.
    /// </summary>
    public bool IsInstalled { get; init; }
}
