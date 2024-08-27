using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.Contracts;

/// <summary>
/// PCI.
/// </summary>
[ComplexType]
public sealed record Pci
{
    /// <summary>
    /// Уникальный идентификатор.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// BUS.
    /// </summary>
    public int Bus { get; set; }
}
