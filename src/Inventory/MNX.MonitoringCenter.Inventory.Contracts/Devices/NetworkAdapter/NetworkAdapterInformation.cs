using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.Contracts.Devices.NetworkAdapter;

/// <summary>
/// Информация о сетевом адаптере.
/// </summary>
[ComplexType]
public record NetworkAdapterInformation
{
    /// <summary>
    /// Производитель.
    /// </summary>
    public string? Manufacturer { get; init; }

    /// <summary>
    /// Модель.
    /// </summary>
    public string? Model { get; init; }

    /// <summary>
    /// Полное название.
    /// </summary>
    public string? Name
    {
        get
        {
            var value = $"{Manufacturer} {Model}";

            if (string.IsNullOrWhiteSpace(value))
                return null;

            return value;
        }
    }

    /// <summary>
    /// Серийный номер.
    /// </summary>
    public string? SerialNumber { get; init; }

    /// <summary>
    /// Код продавца.
    /// </summary>
    public string? de { get; init; }

    /// <summary>
    /// Информация о BUS.
    /// </summary>
    public string? BusInfo { get; init; }

    /// <summary>
    /// Логическое имя.
    /// </summary>
    public required string LogicalName { get; init; }

    /// <summary>
    /// MAC адрес.
    /// </summary>
    public required string Mac { get; init; }
}
