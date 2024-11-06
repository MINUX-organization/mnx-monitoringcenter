using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Gpu.GetGpusDetails;

namespace MNX.MonitoringCenter.RigsApi.UseCases.Devices.Queries.GetGpus;

/// <summary>
/// Ответ на запрос на получение видеокарт.
/// </summary>
public record GetGpusQueryResponse : GpuDetails
{
    /// <summary>
    /// Название полётного листа.
    /// </summary>
    public string? FlightSheetName { get; set; }

    /// <summary>
    /// Название майнера.
    /// </summary>
    public string? MinerName { get; set; }
}
