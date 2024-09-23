using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.Contracts.NetworkAdapter;

/// <summary>
/// Информация о сетевом адаптере.
/// </summary>
[ComplexType]
public record NetworkAdapterInformation
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
    /// Код продавца.
    /// </summary>
    public required string VendorCode { get; init; }

    /// <summary>
    /// Информация о BUS.
    /// </summary>
    public required string BusInfo { get; init; }

    /// <summary>
    /// Логическое имя.
    /// </summary>
    public required string LogicalName { get; init; }

    /// <summary>
    /// MAC адрес.
    /// </summary>
    public required string Mac { get; init; }
}
