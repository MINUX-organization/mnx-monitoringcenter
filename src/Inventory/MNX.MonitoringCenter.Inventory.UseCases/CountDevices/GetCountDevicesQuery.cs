using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Inventory.UseCases.Cpu;
using MNX.MonitoringCenter.Inventory.UseCases.Drive;
using MNX.MonitoringCenter.Inventory.UseCases.Gpu;

namespace MNX.MonitoringCenter.Inventory.UseCases.CountDevices;

/// <summary>
/// Запрос на получение количества устройств инвентаризации.
/// </summary>
public sealed class GetCountDevicesQuery : IRequest<Result<GetCountDevicesQueryResponse>>
{
    /// <summary>
    /// Спецификация.
    /// </summary>
    public DeviceSpecification Specification { get; }

    public GetCountDevicesQuery(Guid userId, Guid[]? rigsIds = null)
    {
        Specification = new DeviceSpecification(userId, rigsIds);
    }
}

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
            .GetCountOFCpusGroupedByManufacturer(request.Specification, cancellationToken);

        var gpusCount = await _gpuRepository
            .GetCountOFGpusGroupedByManufacturer(request.Specification, cancellationToken);

        var drivesCount = await _driveRepository.GetCount(request.Specification, cancellationToken);

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