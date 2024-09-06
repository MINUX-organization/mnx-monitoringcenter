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
    /// Получить кол-во видеокарт по спецификации.
    /// </summary>
    /// <param name="specification">Спецификация.</param>
    /// <returns> Кол-во видеокарт, соответствующих спецификации. </returns>
    Task<int> GetCount(DeviceSpecification specification);
}
