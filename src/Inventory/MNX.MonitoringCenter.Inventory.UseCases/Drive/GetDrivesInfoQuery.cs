using MediatR;
using MNX.Application.UseCases;

namespace MNX.MonitoringCenter.Inventory.UseCases.Drive;

using Drive = Contracts.Drive.Drive;

/// <summary>
/// Запрос на получение списка дисков, работающих на риге.
/// </summary>
/// <param name="RigId"> Идентификатор рига. </param>
public sealed record GetDrivesInfoQuery(Guid RigId) : IRequest<Result<List<Drive>>>;

/// <summary>
/// Обработчик <see cref="GetDrivesInfoQuery"/>.
/// </summary>
public class GetDrivesInfoQueryHandler : IRequestHandler<GetDrivesInfoQuery, Result<List<Drive>>>
{
    private readonly IDriveRepository _driveRepository;

    public GetDrivesInfoQueryHandler(IDriveRepository driveRepository)
    {
        _driveRepository = driveRepository ?? throw new ArgumentNullException(nameof(driveRepository));
    }

    public async Task<Result<List<Drive>>> Handle(GetDrivesInfoQuery request,
                                                  CancellationToken cancellationToken)
    {
        var drives = await _driveRepository.GetList(request.RigId, cancellationToken);

        if (drives == null)
        {
            return Result<List<Drive>>
                .Invalid($"Inventory for rig with id equaled {request.RigId} was not found");
        }

        return Result<List<Drive>>.Success(drives);
    }
}
