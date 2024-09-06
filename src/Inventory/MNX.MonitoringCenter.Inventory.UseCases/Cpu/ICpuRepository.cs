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
    /// Получить срез инвентаризации процессоров за указанный период.
    /// </summary>
    /// <param name="specification"> Спецификация. </param>
    /// <param name="startPeriod"> Начало периода. </param>
    /// <param name="endPeriod"> Конец периода. </param>
    /// <returns> Срез инвентаризации процессоров. </returns>
    IAsyncEnumerable<List<Cpu>> GetSliceForAPeriod(InventorySpecification specification,
                                                   DateTimeOffset startPeriod,
                                                   DateTimeOffset endPeriod);

    /// <summary>
    /// Получить кол-во процессоров по спецификации.
    /// </summary>
    /// <param name="specification"> Спецификация. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Кол-во процессоров, соответствующих спецификации. </returns>
    Task<int> GetCount(DeviceSpecification specification, CancellationToken cancellationToken);
}
