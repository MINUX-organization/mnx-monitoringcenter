using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Information;

/// <summary>
/// Информация о видеокарте.
/// </summary>
[ComplexType]
public record GpuInformation
{
    /// <summary>
    /// Производитель.
    /// </summary>
    public required string Manufacturer { get; init; }

    /// <summary>
    /// Модель.
    /// </summary>
    public string Model { get; init; } = "Unknown";

    /// <summary>
    /// Полное название.
    /// </summary>
    public string Name { get => $"{Manufacturer} {Model}"; }

    /// <summary>
    /// Серийный номер.
    /// </summary>
    public string? SerialNumber { get; init; }

    /// <summary>
    /// Продавец.
    /// </summary>
    public string? Vendor { get; init; }

    /// <summary>
    /// Версия BIOS.
    /// </summary>
    public string? BiosVersion { get; init; }

    /// <summary>
    /// Технология параллельных вычислений.
    /// </summary>
    public required ParallelComputingTechnology Technology { get; init; }

    /// <summary>
    /// Память.
    /// </summary>
    public required MemoryInformation Memory { get; init; }
}
