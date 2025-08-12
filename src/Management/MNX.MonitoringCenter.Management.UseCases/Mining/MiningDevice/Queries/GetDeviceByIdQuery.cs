using MediatR;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.Contracts.MiningDevice;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice.Queries;

/// <summary>
/// Получить майнинг устройство по идентификатору.
/// </summary>
/// <param name="Id"> Идентификатор устройства. </param>
/// <param name="UserId"> Идентификатор пользователя. </param>
public sealed record GetDeviceByIdQuery(Guid Id, Guid UserId) : IRequest<Result<MiningDeviceModel>>;


/// <summary>
/// Обработчик <see cref="GetDeviceByIdQuery"/>.
/// </summary>
public class GetDeviceByIdQueryHandler : IRequestHandler<GetDeviceByIdQuery, Result<MiningDeviceModel>>
{
    private readonly IMiningDeviceMapper _miningDeviceMapper;

    private readonly IMiningDeviceRepository _repository;

    ///
    public GetDeviceByIdQueryHandler(IMiningDeviceMapper miningDeviceMapper, IMiningDeviceRepository repository)
    {
        _miningDeviceMapper = miningDeviceMapper ?? throw new ArgumentNullException(nameof(miningDeviceMapper));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    ///
    public async Task<Result<MiningDeviceModel>> Handle(GetDeviceByIdQuery request, CancellationToken cancellationToken)
    {
        var device = await _repository.GetActiveDeviceById(request.Id, request.UserId, cancellationToken);

        if (device is null)
        {
            return Result<MiningDeviceModel>.Invalid("Mining device wasn't found");
        }

        return Result<MiningDeviceModel>.Success(_miningDeviceMapper.MapToModel(device));
    }
}
