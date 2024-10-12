using MediatR;
using MNX.Application.UseCases;

namespace MNX.MonitoringCenter.Inventory.Contracts.Queries.CountDevices;

/// <summary>
/// Запрос на получение количества устройств инвентаризации.
/// </summary>
public sealed class GetCountDevicesQuery : IRequest<Result<GetCountDevicesQueryResponse>>
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
