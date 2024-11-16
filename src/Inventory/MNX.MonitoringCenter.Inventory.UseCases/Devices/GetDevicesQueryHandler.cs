using MediatR;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices;

namespace MNX.MonitoringCenter.Inventory.UseCases.Devices;

/// <summary>
/// Реализация <see cref="GetDevicesQuery"/>.
/// </summary>
public class GetDevicesQueryHandler : IStreamRequestHandler<GetDevicesQuery, Device>
{
    private readonly IDevicesRepository _devicesRepository;

    public GetDevicesQueryHandler(IDevicesRepository devicesRepository)
    {
        _devicesRepository = devicesRepository
            ?? throw new ArgumentNullException(nameof(devicesRepository));
    }

    public IAsyncEnumerable<Device> Handle(GetDevicesQuery request, CancellationToken cancellationToken)
    {
        return _devicesRepository.GetList(request.Specification, request.DeviceTypes);
    }
}
