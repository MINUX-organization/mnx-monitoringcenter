using MediatR;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Drive;

namespace MNX.MonitoringCenter.Inventory.UseCases.Devices.Drive;

using Drive = Contracts.Devices.Drive.Drive;

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
        var drives = await _driveRepository.GetDrives(request.Specification, cancellationToken);

        if (drives == null)
        {
            return Result<List<Drive>>.Invalid($"Inventory was not found");
        }

        return Result<List<Drive>>.Success(drives);
    }
}
