using MediatR;
using MNX.Application.UseCases.Requests;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.Contracts.Overclocking;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.UseCases.Overclocking;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice.Queries;

/// <summary>
/// Запрос на получение разгона майнинг устройства.
/// </summary>
/// <param name="DeviceId"> Идентификатор устройства. </param>
/// <param name="UserId"> Идентификатор пользователя. </param>
public sealed record GetDeviceOverclockingQuery(Guid DeviceId, Guid UserId)
    : IUserableRequest<Result<IOverclockingModel>>;


/// <summary>
/// Обработчик <see cref="GetDeviceOverclockingQuery"/>.
/// </summary>
public class GetDeviceOverclockingQueryHandler
    : IRequestHandler<GetDeviceOverclockingQuery, Result<IOverclockingModel>>
{
    private readonly IOverclockingModelMapper<IOverclockingModel, IOverclocking> _overclockingMapper;

    private readonly IMiningDeviceRepository _repository;

    ///
    public GetDeviceOverclockingQueryHandler(IMiningDeviceRepository repository,
                                             IOverclockingModelMapper<IOverclockingModel, IOverclocking> mapper)
    {
        _overclockingMapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    ///
    public async Task<Result<IOverclockingModel>> Handle(GetDeviceOverclockingQuery request, CancellationToken cancellationToken)
    {
        var overclocking = await _repository.GetOverclocking(request.DeviceId, request.UserId);

        if (overclocking is null)
        {
            return Result<IOverclockingModel>.Invalid("Mining device wasn't found!");
        }

        return Result<IOverclockingModel>.Success(_overclockingMapper.MapToModel(overclocking));
    }
}
