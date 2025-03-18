using AutoMapper;
using MediatR;
using MNX.Application.UseCases.Requests;
using MNX.MonitoringCenter.Management.Contracts.MiningDevice;
using System.Runtime.CompilerServices;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice.Queries;

/// <summary>
/// Запрос на получение майнинг устройств.
/// </summary>
public sealed class GetAvailableMiningDevicesQuery : IUserableStreamRequest<MiningDeviceModel>
{
    /// <inheritdoc/>
    public Guid UserId { get => Specification.UserId; }

    /// <summary>
    /// Спецификация.
    /// </summary>
    public Specification Specification { get; }

    public GetAvailableMiningDevicesQuery(Guid userId)
    {
        Specification = new Specification(userId);
    }

    public GetAvailableMiningDevicesQuery(Guid userId,
                                          string filterString,
                                          object[] filterParameters)
    {
        Specification = new Specification(userId, filterString, filterParameters);
    }
}


/// <summary>
/// Обработчик <see cref="GetAvailableMiningDevicesQuery"/>.
/// </summary>
public class GetAvailableMiningDevicesQueryHandler
    : IStreamRequestHandler<GetAvailableMiningDevicesQuery, MiningDeviceModel>
{
    private readonly IMapper _mapper;

    private readonly IMiningDeviceRepository _miningDeviceRepository;

    public GetAvailableMiningDevicesQueryHandler(IMapper mapper,
                                                 IMiningDeviceRepository miningDeviceRepository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        _miningDeviceRepository = miningDeviceRepository
            ?? throw new ArgumentNullException(nameof(miningDeviceRepository));
    }

    public async IAsyncEnumerable<MiningDeviceModel> Handle(GetAvailableMiningDevicesQuery request,
                                                           [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var devices = _miningDeviceRepository.GetAvailable(request.Specification);

        await foreach (var device in devices.WithCancellation(cancellationToken))
        {
            yield return _mapper.Map<MiningDeviceModel>(device);
        }
    }
}
