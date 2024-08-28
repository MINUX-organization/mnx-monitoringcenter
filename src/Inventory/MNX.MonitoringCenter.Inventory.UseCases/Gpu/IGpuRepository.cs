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
    /// Получить кол-во видеокарт по спецификации.
    /// </summary>
    /// <param name="specification">Спецификация.</param>
    /// <returns> Кол-во видеокарт, соответствующих спецификации. </returns>
    Task<int> GetCount(DeviceSpecification specification);
}
