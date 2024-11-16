using MediatR;
using MNX.MonitoringCenter.Management.Core.MiningDevice;

namespace MNX.MonitoringCenter.Management.UseCases.MiningDevice.Queries;

/// <summary>
/// Запрос на получение майнинг устройств.
/// </summary>
public sealed class GetAvailableMiningDevicesQuery : IStreamRequest<MiningDeviceInfo>
{
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
    : IStreamRequestHandler<GetAvailableMiningDevicesQuery, MiningDeviceInfo>
{
    private readonly IMiningDeviceRepository _miningDeviceRepository;

    public GetAvailableMiningDevicesQueryHandler(IMiningDeviceRepository miningDeviceRepository)
    {
        _miningDeviceRepository = miningDeviceRepository
            ?? throw new ArgumentNullException(nameof(miningDeviceRepository));
    }

    public IAsyncEnumerable<MiningDeviceInfo> Handle(GetAvailableMiningDevicesQuery request,
                                                     CancellationToken cancellationToken)
    {
        return _miningDeviceRepository.GetAvailable(request.Specification);
    }
}
