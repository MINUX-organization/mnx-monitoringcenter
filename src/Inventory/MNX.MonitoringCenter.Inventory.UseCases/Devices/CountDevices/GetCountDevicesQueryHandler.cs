using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.CountDevices;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.CountDevices;
using MNX.MonitoringCenter.Inventory.UseCases.Devices.Cpu;
using MNX.MonitoringCenter.Inventory.UseCases.Devices.Drive;
using MNX.MonitoringCenter.Inventory.UseCases.Devices.Gpu;

namespace MNX.MonitoringCenter.Inventory.UseCases.Devices.CountDevices;

/// <summary>
/// Реализация <see cref="GetCountDevicesQuery"/>.
/// </summary>
public class GetCountDevicesQueryHandler : IRequestHandler<GetCountDevicesQuery, Result<ModelWithCountDevices>>
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

    public async Task<Result<ModelWithCountDevices>> Handle(GetCountDevicesQuery request,
                                                                   CancellationToken cancellationToken)
    {
        var cpusCount = await _cpuRepository
            .GetCpusCountGroupedByManufacturer(request.Specification, cancellationToken);

        var gpusCount = await _gpuRepository
            .GetGpusCountGroupedByManufacturer(request.Specification, cancellationToken);

        var drivesCount = await _driveRepository.GetDrivesCount(request.Specification, cancellationToken);

        return Result<ModelWithCountDevices>.Success(new ModelWithCountDevices()
        {
            TotalCpusCountGroupedByManufacturer = cpusCount,
            TotalCpusCount = cpusCount.Values.Sum(),
            TotalGpusCountGroupedByManufacturer = gpusCount,
            TotalGpusCount = gpusCount.Values.Sum(),
            TotalDrivesCount = drivesCount
        });
    }
}