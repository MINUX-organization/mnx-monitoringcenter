using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Inventory.Contracts;

namespace MNX.MonitoringCenter.Inventory.UseCases.Software;

/// <summary>
/// Запрос на получение информации о программном обеспечении.
/// </summary>
public sealed record GetSoftwareInfoQuery : IRequest<Result<SoftwareInventory>>
{
    /// <summary>
    /// Спецификация инвентаризации.
    /// </summary>
    public InventorySpecification Specification { get; }

    public GetSoftwareInfoQuery(Guid userId, Guid rigId)
    {
        Specification = new InventorySpecification(userId, rigId);
    }
}

/// <summary>
/// Обработчик <see cref="GetSoftwareInfoQuery"/>.
/// </summary>
public class GetSoftwareInfoQueryHandler : IRequestHandler<GetSoftwareInfoQuery, Result<SoftwareInventory>>
{
    private readonly ISoftwareRepository _repository;

    public GetSoftwareInfoQueryHandler(ISoftwareRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Result<SoftwareInventory>> Handle(GetSoftwareInfoQuery request,
                                                        CancellationToken cancellationToken)
    {
        var software = await _repository.GetByRigId(request.Specification, cancellationToken);

        if (software == null)
        {
            return Result<SoftwareInventory>.Invalid($"Inventory was not found");
        }

        return Result<SoftwareInventory>.Success(software);
    }
}
