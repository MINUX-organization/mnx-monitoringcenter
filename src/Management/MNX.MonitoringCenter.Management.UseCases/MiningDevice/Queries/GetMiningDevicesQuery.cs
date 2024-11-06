using MediatR;
using MNX.MonitoringCenter.Management.Core.MiningDevice;

namespace MNX.MonitoringCenter.Management.UseCases.MiningDevice.Queries;

/// <summary>
/// Запрос на получение майнинг устройств.
/// </summary>
public sealed class GetMiningDevicesQuery : IStreamRequest<MiningDeviceInfo>
{
    /// <summary>
    /// Спецификация.
    /// </summary>
    public Specification Specification { get; }

    public GetMiningDevicesQuery(Guid userId, Guid[]? devicesIds = null)
    {
        Specification = new Specification(userId, devicesIds);
    }

    public GetMiningDevicesQuery(Guid userId, Guid[]? devicesIds,
                                 string filterString, string[] filterParameters)
    {
        Specification = new Specification(userId, devicesIds, filterString, filterParameters);
    }
}


/// <summary>
/// Обработчик <see cref="GetMiningDevicesQuery"/>.
/// </summary>
public class GetMiningDevicesQueryHandler : IStreamRequestHandler<GetMiningDevicesQuery, MiningDeviceInfo>
{
    private readonly IMiningDeviceRepository _miningDeviceRepository;

    public GetMiningDevicesQueryHandler(IMiningDeviceRepository miningDeviceRepository)
    {
        _miningDeviceRepository = miningDeviceRepository
            ?? throw new ArgumentNullException(nameof(miningDeviceRepository));
    }

    public IAsyncEnumerable<MiningDeviceInfo> Handle(GetMiningDevicesQuery request,
                                                     CancellationToken cancellationToken)
    {
        return _miningDeviceRepository.GetAvailable(request.Specification);
    }
}
