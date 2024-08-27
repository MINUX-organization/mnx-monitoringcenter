using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.Contracts.Drive;

/// <summary>
/// Информация о жёстком диске.
/// </summary>
[ComplexType]
public sealed record DriveInformation
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
    /// Вместимость.
    /// </summary>
    public int Capacity { get; set; }
}
