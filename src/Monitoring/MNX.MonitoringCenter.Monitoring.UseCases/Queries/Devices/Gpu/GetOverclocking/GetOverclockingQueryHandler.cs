using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Monitoring.Core.Devices.Gpu;
using MNX.MonitoringCenter.Monitoring.UseCases.Abstractions;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Queries.Devices.Gpu.GetOverclocking;

/// <summary>
/// Обработчик запроса на получение разгона видеокарты по идентификатору.
/// </summary>
public class GetOverclockingQueryHandler : IRequestHandler<GetGpuOverclockingQuery, Result<GpuOverclocking?>>
{
    private readonly IGpuRepository _gpuRepository;

    public GetOverclockingQueryHandler(IGpuRepository gpuRepository)
    {
        _gpuRepository = gpuRepository ?? throw new ArgumentNullException(nameof(gpuRepository));
    }

    public async Task<Result<GpuOverclocking?>> Handle(GetGpuOverclockingQuery request, CancellationToken cancellationToken)
    {
        var gpu = await _gpuRepository.GetById(request.GpuId);

        if (gpu is null)
        {
            return Result<GpuOverclocking?>.Invalid($"Gpu with ID equaled {request.GpuId} was not found!");
        }

        return Result<GpuOverclocking?>.Success(await _gpuRepository.GetOverclocking(request.GpuId));
    }
}
