using MediatR;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.CountDevices;

namespace MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.CountDevices;

/// <summary>
/// Запрос на получение количества устройств инвентаризации.
/// </summary>
public sealed class GetCountDevicesQuery : IRequest<Result<ModelWithCountDevices>>
{
    /// <summary>
    /// Спецификация.
    /// </summary>
    public DeviceSpecification Specification { get; }

    public GetCountDevicesQuery(Guid userId, Guid rigId)
    {
        Specification = new DeviceSpecification(userId, new Guid[] { rigId });
    }

    public GetCountDevicesQuery(Guid userId, Guid[]? rigsIds = null)
    {
        Specification = new DeviceSpecification(userId, rigsIds);
    }
}
