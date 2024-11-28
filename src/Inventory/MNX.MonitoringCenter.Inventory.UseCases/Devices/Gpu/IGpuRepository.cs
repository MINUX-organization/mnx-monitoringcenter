using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Restrictions;
using MNX.MonitoringCenter.Inventory.Contracts.Requests;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Gpu.GetGpusDetails;

namespace MNX.MonitoringCenter.Inventory.UseCases.Devices.Gpu;

using Gpu = Contracts.Devices.Gpu.Gpu;

/// <summary>
/// Репозиторий для доступа к видеокартам.
/// </summary>
public interface IGpuRepository
{
    /// <summary>
    /// Получить список видеокарт.
    /// </summary>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Список видеокарт. </returns>
    IAsyncEnumerable<GpuDetails> GetGpus(DeviceSpecification specification);

    /// <summary>
    /// Получить срез инвентаризации видеокарт за указанный период.
    /// </summary>
    /// <param name="specification"> Спецификация. </param>
    /// <param name="startPeriod"> Начало периода. </param>
    /// <param name="endPeriod"> Конец периода. </param>
    /// <returns> Срез инвентаризации видеокарт. </returns>
    IAsyncEnumerable<List<Gpu>> GetGpusSliceForAPeriod(DeviceSpecification specification,
                                                       DateTimeOffset startPeriod,
                                                       DateTimeOffset endPeriod);

    /// <summary>
    /// Получить уникальные полные названия видеокарт.
    /// </summary>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Уникальные названия видеокарт. </returns>
    IAsyncEnumerable<string> GetGpusUniqueNames(DeviceSpecification specification);

    /// <summary>
    /// Получение ограничений по названию видеокарты.
    /// </summary>
    /// <param name="gpuName"> Полное название видеокарты. </param>
    /// <returns> Ограничения. </returns>
    Task<GpuRestrictions?> GetGpusRestrictions(string gpuName);

    /// <summary>
    /// Получить кол-во видеокарт по спецификации.
    /// </summary>
    /// <param name="specification"> Спецификация. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Кол-во видеокарт, соответствующих спецификации. </returns>
    Task<int> GetGpusCount(DeviceSpecification specification, CancellationToken cancellationToken);

    /// <summary>
    /// Получить кол-во видеокарт по спецификации, сгруппированных по производителю.
    /// </summary>
    /// <param name="specification"> Спецификация. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Словарь, в котором key - производитель, value - кол-во. </returns>
    Task<Dictionary<string, int>> GetGpusCountGroupedByManufacturer(DeviceSpecification specification,
                                                                    CancellationToken cancellationToken);
}
