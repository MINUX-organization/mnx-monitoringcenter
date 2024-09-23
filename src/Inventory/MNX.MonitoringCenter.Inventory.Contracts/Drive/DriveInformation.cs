using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.Contracts.Drive;

/// <summary>
/// Информация о жёстком диске.
/// </summary>
[ComplexType]
public record DriveInformation
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
    /// Полное название.
    /// </summary>
    public string Name { get => $"{Manufacturer} {Model}"; }

    /// <summary>
    /// Серийный номер.
    /// </summary>
    public required string SerialNumber { get; init; }

    /// <summary>
    /// Вместимость.
    /// </summary>
    public int Capacity { get; init; }
}
