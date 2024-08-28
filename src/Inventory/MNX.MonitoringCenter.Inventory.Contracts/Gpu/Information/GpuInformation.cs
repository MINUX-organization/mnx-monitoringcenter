using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.Contracts.Gpu.Information;

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
    public required string Model { get; init; }

    /// <summary>
    /// Серийный номер.
    /// </summary>
    public required string SerialNumber { get; init; }

    /// <summary>
    /// Продавец.
    /// </summary>
    public required string Vendor { get; init; }

    /// <summary>
    /// Версия BIOS.
    /// </summary>
    public required string BiosVersion { get; init; }

    /// <summary>
    /// Драйвер.
    /// </summary>
    public required string DriverVersion { get; init; }

    /// <summary>
    /// Технология параллельных вычислений.
    /// </summary>
    public required ParallelComputingTechnology Technology { get; init; }

    /// <summary>
    /// Память.
    /// </summary>
    public required MemoryInformation Memory { get; init; }
}
