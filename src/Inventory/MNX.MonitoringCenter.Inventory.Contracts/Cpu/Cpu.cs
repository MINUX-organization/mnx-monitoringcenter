namespace MNX.MonitoringCenter.Inventory.Contracts.Cpu;

/// <summary>
/// Процессор.
/// </summary>
public record Cpu
{
    /// <summary>
    /// Уникальный идентификатор.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// PCI.
    /// </summary>
    public required Pci Pci { get; init; }

    /// <summary>
    /// Информация.
    /// </summary>
    public required CpuInformation Information { get; init; }

    /// <summary>
    /// Ограничения.
    /// </summary>
    public required CpuRestrictions Restrictions { get; init; }
}
