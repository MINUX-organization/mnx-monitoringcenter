using MNX.MonitoringCenter.Inventory.Contracts.Devices;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Information;

namespace MNX.MonitoringCenter.RigsApi.UseCases.Devices.Queries.GetGpus;

/// <summary>
/// Ответ на запрос на получение видеокарт.
/// </summary>
public class GetGpusQueryResponse
{
    /// <summary>
    /// Уникальный идентификатор.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// PCI.
    /// </summary>
    public required Pci Pci { get; init; }

    /// <summary>
    /// Информация.
    /// </summary>
    public required GpuInformation Information { get; init; }

    /// <summary>
    /// Название рига.
    /// </summary>
    public required string RigName { get; init; }

    /// <summary>
    /// Версия драйвера для работы с видеокартой.
    /// </summary>
    public string? DriverVersion { get; init; }

    /// <summary>
    /// Название полётного листа.
    /// </summary>
    public string? FlightSheetName { get; init; }

    /// <summary>
    /// Название майнера.
    /// </summary>
    public string? MinerName { get; init; }
}
