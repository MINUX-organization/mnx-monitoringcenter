using MediatR;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Inventory.Contracts;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Software;

namespace MNX.MonitoringCenter.Inventory.UseCases.Software;

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
        var software = await _repository.GetSoftwareByRigId(request.RigId, request.UserId, cancellationToken);

        if (software == null)
        {
            return Result<SoftwareInventory>.Invalid($"Inventory was not found");
        }

        return Result<SoftwareInventory>.Success(software);
    }
}
