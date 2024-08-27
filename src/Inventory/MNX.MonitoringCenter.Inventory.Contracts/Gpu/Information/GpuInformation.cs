using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.Contracts.Gpu.Information;

/// <summary>
/// Информация о видеокарте.
/// </summary>
[ComplexType]
public sealed record GpuInformation
{
    /// <summary>
    /// Производитель.
    /// </summary>
    public GpuManufacturerEnum Manufacturer { get; set; }

    /// <summary>
    /// Модель.
    /// </summary>
    public string Model { get; set; }

    /// <summary>
    /// Серийный номер.
    /// </summary>
    public string SerialNumber { get; set; }

    /// <summary>
    /// Продавец.
    /// </summary>
    public string Vendor { get; set; }

    /// <summary>
    /// Версия BIOS.
    /// </summary>
    public string BiosVersion { get; set; }

    /// <summary>
    /// Драйвер.
    /// </summary>
    public string DriverVersion { get; set; }

    /// <summary>
    /// Технология параллельных вычислений.
    /// </summary>
    public ParallelComputingTechnology Technology { get; set; }

    /// <summary>
    /// Память.
    /// </summary>
    public MemoryInformation Memory { get; set; }
}
