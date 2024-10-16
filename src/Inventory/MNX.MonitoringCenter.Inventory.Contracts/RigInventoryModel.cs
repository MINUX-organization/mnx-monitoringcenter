namespace MNX.MonitoringCenter.Inventory.Contracts;

/// <summary>
/// Модель инвентаризации рига.
/// </summary>
public sealed record RigInventoryModel
{
    /// <summary>
    /// Процессоры.
    /// </summary>
    public List<Cpu.Cpu> Cpus { get; init; } = new(0);

    /// <summary>
    /// Видеокарты.
    /// </summary>
    public List<Gpu.Gpu> Gpus { get; init; } = new(0);

    /// <summary>
    /// Диски.
    /// </summary>
    public List<Drive.Drive> Drives { get; init; } = new(0);

    /// <summary>
    /// Сетевые адаптеры.
    /// </summary>
    public List<NetworkAdapter.NetworkAdapter> NetworkAdapters { get; init; } = new(0);

    /// <summary>
    /// Материнская плата.
    /// </summary>
    public required Motherboard.Motherboard Motherboard { get; init; }

    /// <summary>
    /// Программное обеспечение.
    /// </summary>
    public required SoftwareInventory Software { get; init; }
}
