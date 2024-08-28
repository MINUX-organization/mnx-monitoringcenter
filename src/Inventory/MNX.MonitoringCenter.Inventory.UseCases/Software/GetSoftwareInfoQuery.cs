using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Inventory.Contracts;

namespace MNX.MonitoringCenter.Inventory.UseCases.Software;

/// <summary>
/// Запрос на получение информации о программном обеспечении.
/// </summary>
/// <param name="RigId"> Идентификатор рига. </param>
public sealed record GetSoftwareInfoQuery(Guid RigId) : IRequest<Result<SoftwareInventory>>;

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
        var software = await _repository.GetByRigId(request.RigId, cancellationToken);

        if (software == null)
        {
            return Result<SoftwareInventory>
                .Invalid($"Inventory for rig with id equaled {request.RigId} was not found");
        }

        return Result<SoftwareInventory>.Success(software);
    }
}
