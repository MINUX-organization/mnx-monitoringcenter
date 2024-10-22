using MediatR;

namespace MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Cpu;

using Cpu = Contracts.Devices.Cpu.Cpu;

/// <summary>
/// Запрос на получение среза инвентаризации процессоров за период.
/// </summary>
public class GetSliceCpuInventoryForAPeriodQuery : IStreamRequest<List<Cpu>>
{
    /// <summary>
    /// Спецификация инвентаризации.
    /// </summary>
    public InventorySpecification Specification { get; }

    /// <summary>
    /// Начало периода.
    /// </summary>
    public DateTimeOffset StartPeriod { get; }

    /// <summary>
    /// Конец периода.
    /// </summary>
    public DateTimeOffset EndPeriod { get; }

    public GetSliceCpuInventoryForAPeriodQuery(Guid userId,
                                               Guid[] rigsIds,
                                               DateTimeOffset startPeriod,
                                               DateTimeOffset endPeriod)
    {
        Specification = new InventorySpecification(userId, rigsIds);
        StartPeriod = startPeriod;
        EndPeriod = endPeriod;
    }

    public GetSliceCpuInventoryForAPeriodQuery(Guid userId,
                                               Guid rigId,
                                               DateTimeOffset startPeriod,
                                               DateTimeOffset endPeriod)
        : this(userId, new Guid[] { rigId }, startPeriod, endPeriod) { }
}