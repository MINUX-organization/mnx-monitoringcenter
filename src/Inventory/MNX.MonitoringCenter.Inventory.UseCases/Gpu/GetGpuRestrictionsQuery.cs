using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Inventory.Contracts.Gpu.Restrictions;

namespace MNX.MonitoringCenter.Inventory.UseCases.Gpu;

/// <summary>
/// Запрос на получение ограничений видеокарты.
/// </summary>
/// <param name="GpuName"> Название видеокарты. </param>
public sealed record GetGpuRestrictionsQuery(string GpuName) : IRequest<Result<GpuRestrictions>>;

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
        var restrictions = await _repository.GetRestrictionsByGpuName(request.GpuName);

        if (restrictions is null)
        {
            return Result<GpuRestrictions>.Invalid("Gpu wasn`t found");
        }

        return Result<GpuRestrictions>.Success(restrictions);
    }
}
