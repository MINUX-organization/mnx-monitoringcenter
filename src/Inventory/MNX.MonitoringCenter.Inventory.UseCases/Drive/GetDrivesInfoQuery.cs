using MediatR;
using MNX.Application.UseCases;

namespace MNX.MonitoringCenter.Inventory.UseCases.Drive;

using Drive = Contracts.Drive.Drive;

/// <summary>
/// Запрос на получение списка дисков, работающих на риге.
/// </summary>
public class GetDrivesInfoQuery : IRequest<Result<List<Drive>>>
{
    /// <summary>
    /// Спецификация инвентаризации.
    /// </summary>
    public InventorySpecification Specification { get; }

    public GetDrivesInfoQuery(Guid userId, Guid rigId)
    {
        Specification = new InventorySpecification(userId, rigId);
    }
}

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
        var drives = await _driveRepository.GetList(request.Specification, cancellationToken);

        if (drives == null)
        {
            return Result<List<Drive>>.Invalid($"Inventory was not found");
        }

        return Result<List<Drive>>.Success(drives);
    }
}
