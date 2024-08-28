namespace MNX.MonitoringCenter.Inventory.UseCases.Cpu;

using Cpu = Contracts.Cpu.Cpu;

/// <summary>
/// Репозиторий для доступа к процессорам.
/// </summary>
public interface ICpuRepository
{
    /// <summary>
    /// Получить список процессоров.
    /// </summary>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Список процессоров. </returns>
    IAsyncEnumerable<Cpu> GetList(DeviceSpecification specification);

    /// <summary>
    /// Получить кол-во процессоров по спецификации.
    /// </summary>
    /// <param name="specification">Спецификация.</param>
    /// <returns> Кол-во процессоров, соответствующих спецификации. </returns>
    Task<int> GetCount(DeviceSpecification specification);
}
