using MediatR;

namespace MNX.MonitoringCenter.Inventory.UseCases.Gpu;

/// <summary>
/// Запрос на получение уникальных названий видеокарт.
/// </summary>
public sealed class GetGpuUniqueNamesQuery : IStreamRequest<string>
{
    /// <summary>
    /// Спецификация инвентаризации.
    /// </summary>
    public InventorySpecification Specification { get; }

    public GetGpuUniqueNamesQuery(Guid userId)
    {
        Specification = new InventorySpecification(userId);
    }
}

/// <summary>
/// Обработчик <see cref="GetGpuUniqueNamesQuery"/>.
/// </summary>
public class GetGpuUniqueNamesQueryHandler : IStreamRequestHandler<GetGpuUniqueNamesQuery, string>
{
    private readonly IGpuRepository _repository;

    public GetGpuUniqueNamesQueryHandler(IGpuRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public IAsyncEnumerable<string> Handle(GetGpuUniqueNamesQuery request, CancellationToken cancellationToken)
    {
        return _repository.GetGpuUniqueNames(request.Specification);
    }
}
