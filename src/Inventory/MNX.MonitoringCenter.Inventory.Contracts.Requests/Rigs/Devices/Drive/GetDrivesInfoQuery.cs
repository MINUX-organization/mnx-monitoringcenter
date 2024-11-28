using MediatR;
using MNX.Application.UseCases.Results;

namespace MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Drive;

using Drive = Contracts.Devices.Drive.Drive;

/// <summary>
/// Запрос на получение списка дисков, работающих на риге.
/// </summary>
public class GetDrivesInfoQuery : IRequest<Result<List<Drive>>>
{
    /// <summary>
    /// Спецификация инвентаризации.
    /// </summary>
    public DeviceSpecification Specification { get; }

    public GetDrivesInfoQuery(Guid userId, Guid rigId)
    {
        Specification = new DeviceSpecification(userId, rigId);
    }
}
