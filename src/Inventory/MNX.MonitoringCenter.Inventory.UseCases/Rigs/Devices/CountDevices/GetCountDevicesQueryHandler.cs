using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.CountDevices;
using MNX.MonitoringCenter.Inventory.UseCases.RigInventory.Devices.Cpu;
using MNX.MonitoringCenter.Inventory.UseCases.RigInventory.Devices.Drive;
using MNX.MonitoringCenter.Inventory.UseCases.RigInventory.Devices.Gpu;

namespace MNX.MonitoringCenter.Inventory.UseCases.RigInventory.Devices.CountDevices;

/// <summary>
/// Реализация <see cref="GetCountDevicesQuery"/>.
/// </summary>
public class GetCountDevicesQueryHandler : IRequestHandler<GetCountDevicesQuery, Result<GetCountDevicesQueryResponse>>
{
    private readonly ICpuRepository _cpuRepository;

    private readonly IDriveRepository _driveRepository;

    private readonly IGpuRepository _gpuRepository;

    public GetCountDevicesQueryHandler(ICpuRepository cpuRepository,
                                       IDriveRepository driveRepository,
                                       IGpuRepository gpuRepository)
    {
        _cpuRepository = cpuRepository ?? throw new ArgumentNullException(nameof(cpuRepository));
        _driveRepository = driveRepository ?? throw new ArgumentNullException(nameof(driveRepository));
        _gpuRepository = gpuRepository ?? throw new ArgumentNullException(nameof(gpuRepository));
    }

    public async Task<Result<GetCountDevicesQueryResponse>> Handle(GetCountDevicesQuery request,
                                                                   CancellationToken cancellationToken)
    {
        var cpusCount = await _cpuRepository
            .GetCpusCountGroupedByManufacturer(request.Specification, cancellationToken);

        var gpusCount = await _gpuRepository
            .GetGpusCountGroupedByManufacturer(request.Specification, cancellationToken);

        var drivesCount = await _driveRepository.GetDrivesCount(request.Specification, cancellationToken);

        return Result<GetCountDevicesQueryResponse>.Success(new GetCountDevicesQueryResponse()
        {
            TotalCpusCountGroupedByManufacturer = cpusCount,
            TotalCpusCount = cpusCount.Values.Sum(),
            TotalGpusCountGroupedByManufacturer = gpusCount,
            TotalGpusCount = gpusCount.Values.Sum(),
            TotalDrivesCount = drivesCount
        });
    }
}