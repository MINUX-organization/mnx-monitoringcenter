using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Entities.Information;

/// <summary>
/// Информация о видеокарте.
/// </summary>
[Owned]
public class GpuInformationInventory
{
    /// <summary>
    /// Производитель.
    /// </summary>
    [Column("manufacturer")]
    public required string Manufacturer { get; init; }

    /// <summary>
    /// Модель.
    /// </summary>
    [Column("model")]
    public string Model { get; init; } = "Unknown";

    /// <summary>
    /// Полное название.
    /// </summary>
    public string Name { get => $"{Manufacturer} {Model}"; }

    /// <summary>
    /// Серийный номер.
    /// </summary>
    [Column("serial_number")]
    public string? SerialNumber { get; init; }

    /// <summary>
    /// Продавец.
    /// </summary>
    [Column("vendor")]
    public string? Vendor { get; init; }

    /// <summary>
    /// Версия BIOS.
    /// </summary>
    [Column("bios_version")]
    public string? BiosVersion { get; init; }

    /// <summary>
    /// Технологии параллельных вычислений.
    /// </summary>
    public required ParallelComputingTechnologyInventory Technology { get; init; }

    /// <summary>
    /// Информация о памяти.
    /// </summary>
    public required MemoryInformationInventory Memory { get; init; }

    /// <summary>
    /// Получить производителя в качестве значения перечисления.
    /// </summary>
    /// <returns>
    /// Значение перечисления <see cref="SupportedGpuManufacturerEnum"/>.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// Производитель не поддерживается.
    /// </exception>
    public SupportedGpuManufacturerEnum GetManufacturerEnumValue()
    {
        if (Enum.TryParse<SupportedGpuManufacturerEnum>(Manufacturer, ignoreCase: true, out var result))
        {
            return result;
        }
        else
        {
            throw new NotSupportedException("This manufacturer does not support");
        }
    }
}
