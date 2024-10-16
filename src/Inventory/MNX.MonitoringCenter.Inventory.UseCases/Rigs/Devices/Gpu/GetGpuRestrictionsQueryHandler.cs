using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Inventory.Contracts.Gpu.Restrictions;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Gpu;

namespace MNX.MonitoringCenter.Inventory.UseCases.RigInventory.Devices.Gpu;

/// <summary>
/// Обработчик <see cref="GetGpuRestrictionsQuery"/>.
/// </summary>
public class GetGpuRestrictionsQueryHandler : IRequestHandler<GetGpuRestrictionsQuery, Result<GpuRestrictions>>
{
    private readonly IGpuRepository _repository;

    public GetGpuRestrictionsQueryHandler(IGpuRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Result<GpuRestrictions>> Handle(GetGpuRestrictionsQuery request, CancellationToken cancellationToken)
    {
        var restrictions = await _repository.GetGpusRestrictions(request.GpuName);

        if (restrictions is null)
        {
            return Result<GpuRestrictions>.Invalid("Gpu wasn`t found");
        }

        return Result<GpuRestrictions>.Success(restrictions);
    }
}
