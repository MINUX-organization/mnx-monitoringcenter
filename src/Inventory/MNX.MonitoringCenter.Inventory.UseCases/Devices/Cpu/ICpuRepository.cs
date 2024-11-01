using MNX.MonitoringCenter.Inventory.Contracts.Requests;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Cpu.GetCpusDetails;

namespace MNX.MonitoringCenter.Inventory.UseCases.Devices.Cpu;

using Cpu = Contracts.Devices.Cpu.Cpu;

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
    IAsyncEnumerable<CpuDetails> GetCpus(DeviceSpecification specification);

    /// <summary>
    /// Получить срез инвентаризации процессоров за указанный период.
    /// </summary>
    /// <param name="specification"> Спецификация. </param>
    /// <param name="startPeriod"> Начало периода. </param>
    /// <param name="endPeriod"> Конец периода. </param>
    /// <returns> Срез инвентаризации процессоров. </returns>
    IAsyncEnumerable<List<Cpu>> GetCpusSliceForAPeriod(DeviceSpecification specification,
                                                       DateTimeOffset startPeriod,
                                                       DateTimeOffset endPeriod);

    /// <summary>
    /// Получить кол-во процессоров по спецификации.
    /// </summary>
    /// <param name="specification"> Спецификация. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Кол-во процессоров, соответствующих спецификации. </returns>
    Task<int> GetCpusCount(DeviceSpecification specification, CancellationToken cancellationToken);

    /// <summary>
    /// Получить кол-во процессоров по спецификации, сгруппированных по производителю.
    /// </summary>
    /// <param name="specification"> Спецификация. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Словарь, в котором key - производитель, value - кол-во. </returns>
    Task<Dictionary<string, int>> GetCpusCountGroupedByManufacturer(DeviceSpecification specification,
                                                                      CancellationToken cancellationToken);
}
