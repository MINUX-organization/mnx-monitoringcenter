using MNX.MonitoringCenter.Inventory.Contracts.Gpu.Restrictions;
using MNX.MonitoringCenter.Inventory.Contracts.Queries;

namespace MNX.MonitoringCenter.Inventory.UseCases.Gpu;

using Gpu = Contracts.Gpu.Gpu;

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
    IAsyncEnumerable<Gpu> GetList(DeviceSpecification specification);

    /// <summary>
    /// Получить срез инвентаризации видеокарт за указанный период.
    /// </summary>
    /// <param name="specification"> Спецификация. </param>
    /// <param name="startPeriod"> Начало периода. </param>
    /// <param name="endPeriod"> Конец периода. </param>
    /// <returns> Срез инвентаризации видеокарт. </returns>
    IAsyncEnumerable<List<Gpu>> GetSliceForAPeriod(InventorySpecification specification,
                                                   DateTimeOffset startPeriod,
                                                   DateTimeOffset endPeriod);

    /// <summary>
    /// Получить уникальные полные названия видеокарт.
    /// </summary>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Уникальные названия видеокарт. </returns>
    IAsyncEnumerable<string> GetGpuUniqueNames(InventorySpecification specification);

    /// <summary>
    /// Получение ограничений по названию видеокарты.
    /// </summary>
    /// <param name="gpuName"> Полное название видеокарты. </param>
    /// <returns> Ограничения. </returns>
    Task<GpuRestrictions?> GetRestrictionsByGpuName(string gpuName);

    /// <summary>
    /// Получить кол-во видеокарт по спецификации.
    /// </summary>
    /// <param name="specification"> Спецификация. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Кол-во видеокарт, соответствующих спецификации. </returns>
    Task<int> GetCount(DeviceSpecification specification, CancellationToken cancellationToken);

    /// <summary>
    /// Получить кол-во видеокарт по спецификации, сгруппированных по производителю.
    /// </summary>
    /// <param name="specification"> Спецификация. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Словарь, в котором key - производитель, value - кол-во. </returns>
    Task<Dictionary<string, int>> GetCountOfGpusGroupedByManufacturer(DeviceSpecification specification,
                                                                      CancellationToken cancellationToken);
}
