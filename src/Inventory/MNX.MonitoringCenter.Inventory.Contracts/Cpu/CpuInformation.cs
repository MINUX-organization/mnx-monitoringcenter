using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.Contracts.Cpu;

/// <summary>
/// Информация о процессоре.
/// </summary>
[ComplexType]
public record CpuInformation
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
    /// Количество ядер.
    /// </summary>
    public int CoresCount { get; init; }

    /// <summary>
    /// Количество потоков.
    /// </summary>
    public int ThreadsCount { get; init; }

    /// <summary>
    /// Архитектура.
    /// </summary>
    public required string Architecture { get; init; }

    /// <summary>
    /// Кэш.
    /// </summary>
    public required CpuCache Cache { get; init; }
}
