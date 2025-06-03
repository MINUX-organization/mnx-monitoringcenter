using MediatR;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Gpu.GetGpuInfo;

namespace MNX.MonitoringCenter.Inventory.UseCases.Devices.Gpu.QueryHandlers;

/// <summary>
/// Обработчик <see cref="GetGpuInfoQuery"/>.
/// </summary>
public class GetGpuInfoQueryHandler : IRequestHandler<GetGpuInfoQuery, Result<GpuInfo>>
{
    private readonly IGpuRepository _repository;

    public GetGpuInfoQueryHandler(IGpuRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Result<GpuInfo>> Handle(GetGpuInfoQuery request, CancellationToken cancellationToken)
    {
        var gpuInfo = await _repository.GetInfo(request.GpuId, request.UserId);

        if (gpuInfo is null)
        {
            return Result<GpuInfo>.Invalid("Gpu was not found");
        }

        return Result<GpuInfo>.Success(gpuInfo);
    }
}
