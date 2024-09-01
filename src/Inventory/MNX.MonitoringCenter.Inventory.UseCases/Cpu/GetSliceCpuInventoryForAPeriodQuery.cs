using MediatR;

namespace MNX.MonitoringCenter.Inventory.UseCases.Cpu;

using Cpu = Contracts.Cpu.Cpu;

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


/// <summary>
/// Обработчик <see cref="GetSliceCpuInventoryForAPeriodQuery"/>.
/// </summary>
public class GetSliceCpuInventoryForAPeriodQueryHandler
    : IStreamRequestHandler<GetSliceCpuInventoryForAPeriodQuery, List<Cpu>>
{
    private readonly ICpuRepository _cpuRepository;

    public GetSliceCpuInventoryForAPeriodQueryHandler(ICpuRepository cpuRepository)
    {
        _cpuRepository = cpuRepository ?? throw new ArgumentNullException(nameof(cpuRepository));
    }

    public IAsyncEnumerable<List<Cpu>> Handle(GetSliceCpuInventoryForAPeriodQuery request,
                                              CancellationToken cancellationToken)
    {
        return _cpuRepository.GetSliceForAPeriod(request.Specification, request.StartPeriod, request.EndPeriod);
    }
}
