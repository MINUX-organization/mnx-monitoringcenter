using MNX.MonitoringCenter.Inventory.Contracts.Devices;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Cpu;

namespace MNX.MonitoringCenter.RigsApi.UseCases.Devices.Queries.GetCpus;

/// <summary>
/// Ответ на запрос на получения списка процессоров.
/// </summary>
public class GetCpusQueryResponse
{
    /// <summary>
    /// Уникальный идентификатор.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// PCI.
    /// </summary>
    public Pci? Pci { get; init; }

    /// <summary>
    /// Информация.
    /// </summary>
    public CpuInformation? Information { get; init; }

    /// <summary>
    /// Название рига.
    /// </summary>
    public string? RigName { get; init; }

    /// <summary>
    /// Название полётного листа.
    /// </summary>
    public string? FlightSheetName { get; init; }

    /// <summary>
    /// Наименование пресета.
    /// </summary>
    public string? PresetName { get; init; }

    /// <summary>
    /// Название майнера.
    /// </summary>
    public string? MinerName { get; init; }

    /// <summary>
    /// Признак нахождения процессора в сети.
    /// </summary>
    public bool IsOnline { get; init; }
}
