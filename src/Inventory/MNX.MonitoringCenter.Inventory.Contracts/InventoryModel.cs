namespace MNX.MonitoringCenter.Inventory.Contracts;

/// <summary>
/// Модель инвентаризации.
/// </summary>
public sealed record InventoryModel
{
    /// <summary>
    /// Процессоры.
    /// </summary>
    public List<Cpu.Cpu> Cpus { get; init; } = new();

    /// <summary>
    /// Видеокарты.
    /// </summary>
    public List<Gpu.Gpu> Gpus { get; init; } = new();

    /// <summary>
    /// Диски.
    /// </summary>
    public List<Drive.Drive> Drives { get; init; } = new();

    /// <summary>
    /// Сетевые адаптеры.
    /// </summary>
    public List<NetworkAdapter.NetworkAdapter> NetworkAdapters { get; init; } = new();

    /// <summary>
    /// Материнская плата.
    /// </summary>
    public required Motherboard.Motherboard Motherboard { get; init; }

    /// <summary>
    /// Программное обеспечение.
    /// </summary>
    public required SoftwareInventory Software { get; init; }
}
