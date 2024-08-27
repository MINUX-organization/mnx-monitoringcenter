using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.Contracts.Cpu;

/// <summary>
/// Информация.
/// </summary>
[ComplexType]
public sealed record CpuInformation
{
    /// <summary>
    /// Производитель.
    /// </summary>
    public CpuManufacturerEnum Manufacturer { get; set; }

    /// <summary>
    /// Модель.
    /// </summary>
    public string Model { get; set; }

    /// <summary>
    /// Количество ядер.
    /// </summary>
    public int CoresCount { get; set; }

    /// <summary>
    /// Количество потоков.
    /// </summary>
    public int ThreadsCount { get; set; }

    /// <summary>
    /// Архитектура.
    /// </summary>
    public string Architecture { get; set; }

    /// <summary>
    /// Кэш.
    /// </summary>
    public CpuCache Cache { get; set; }
}
