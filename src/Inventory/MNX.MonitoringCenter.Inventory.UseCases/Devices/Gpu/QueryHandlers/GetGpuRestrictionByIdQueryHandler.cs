using MediatR;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Restrictions;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Gpu;

namespace MNX.MonitoringCenter.Inventory.UseCases.Devices.Gpu.QueryHandlers;

/// <summary>
/// Обработчик <see cref="GetGpuRestrictionsByIdQuery"/>.
/// </summary>
public class GetGpuRestrictionsByIdQueryHandler : IRequestHandler<GetGpuRestrictionsByIdQuery, Result<GpuRestrictions>>
{
    private readonly IGpuRepository _repository;

    ///
    public GetGpuRestrictionsByIdQueryHandler(IGpuRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    ///
    public async Task<Result<GpuRestrictions>> Handle(GetGpuRestrictionsByIdQuery request, CancellationToken cancellationToken)
    {
        var restrictions = await _repository.GetGpusRestrictionsById(request.GpuId);

        if (restrictions is null)
        {
            return Result<GpuRestrictions>.Invalid("Gpu wasn`t found");
        }

        return Result<GpuRestrictions>.Success(restrictions);
    }
}
