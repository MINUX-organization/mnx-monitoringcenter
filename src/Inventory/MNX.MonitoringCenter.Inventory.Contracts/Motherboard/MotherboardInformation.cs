using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.Contracts.Motherboard;

/// <summary>
/// Информация о материнской плате.
/// </summary>
[ComplexType]
public sealed record MotherboardInformation
{
    /// <summary>
    /// Производитель.
    /// </summary>
    public string Manufacturer { get; set; }

    /// <summary>
    /// Модель.
    /// </summary>
    public string Model { get; set; }

    /// <summary>
    /// Серийный номер.
    /// </summary>
    public string SerialNumber { get; set; }

    /// <summary>
    /// Кол-во Sata портов.
    /// </summary>
    public int SataPortsCount { get; set; }

    /// <summary>
    /// Кол-во портов для плашек операивной памяти.
    /// </summary>
    public int RamPortsCount { get; set; }

    /// <summary>
    /// Кол-во PciX4 портов.
    /// </summary>
    public int PciX4PosrtsCount { get; set; }

    /// <summary>
    /// Кол-во PciX16 портов.
    /// </summary>
    public int PciX16PosrtsCount { get; set; }

}
