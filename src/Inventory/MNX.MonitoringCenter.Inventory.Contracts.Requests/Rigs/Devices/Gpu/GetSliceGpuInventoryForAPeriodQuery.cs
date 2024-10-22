using MediatR;

namespace MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Gpu;

using Gpu = Contracts.Devices.Gpu.Gpu;

/// <summary>
/// Запрос на получение среза инвентаризации видеокарт за период.
/// </summary>
public sealed record GetSliceGpuInventoryForAPeriodQuery : IStreamRequest<List<Gpu>>
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

    public GetSliceGpuInventoryForAPeriodQuery(Guid userId,
                                               Guid[] rigsIds,
                                               DateTimeOffset startPeriod,
                                               DateTimeOffset endPeriod)
    {
        Specification = new InventorySpecification(userId, rigsIds);
        StartPeriod = startPeriod;
        EndPeriod = endPeriod;
    }

    public GetSliceGpuInventoryForAPeriodQuery(Guid userId,
                                               Guid rigId,
                                               DateTimeOffset startPeriod,
                                               DateTimeOffset endPeriod)
        : this(userId, new Guid[] { rigId }, startPeriod, endPeriod) { }
}