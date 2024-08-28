using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.Contracts.Motherboard;

/// <summary>
/// Информация о материнской плате.
/// </summary>
[ComplexType]
public record MotherboardInformation
{
    /// <summary>
    /// Производитель.
    /// </summary>
    public required string Manufacturer { get; init; }

    /// <summary>
    /// Модель.
    /// </summary>
    public required string Model { get; init; }

    /// <summary>
    /// Серийный номер.
    /// </summary>
    public required string SerialNumber { get; init; }

    /// <summary>
    /// Кол-во Sata портов.
    /// </summary>
    public int SataPortsCount { get; init; }

    /// <summary>
    /// Кол-во портов для плашек оперативной памяти.
    /// </summary>
    public int RamPortsCount { get; init; }

    /// <summary>
    /// Кол-во PciX4 портов.
    /// </summary>
    public int PciX4PosrtsCount { get; init; }

    /// <summary>
    /// Кол-во PciX16 портов.
    /// </summary>
    public int PciX16PosrtsCount { get; init; }

}
