namespace MNX.MonitoringCenter.Inventory.Contracts.Cpu;

/// <summary>
/// Процессор.
/// </summary>
public class Cpu
{
    /// <summary>
    /// Уникальный идентификатор.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// PCI.
    /// </summary>
    public Pci Pci { get; set; }

    /// <summary>
    /// Информация.
    /// </summary>
    public CpuInformation Information { get; set; }

    /// <summary>
    /// Ограничения.
    /// </summary>
    public CpuRestrictions Restrictions { get; set; }
}
