namespace MNX.MonitoringCenter.Inventory.UseCases.Gpu;

using Gpu = Contracts.Gpu.Gpu;

public interface IGpuRepository
{
    /// <summary>
    /// Получить список видеокарт.
    /// </summary>
    /// <returns> Список процессоров. </returns>
    IAsyncEnumerable<Gpu> GetList();
}
