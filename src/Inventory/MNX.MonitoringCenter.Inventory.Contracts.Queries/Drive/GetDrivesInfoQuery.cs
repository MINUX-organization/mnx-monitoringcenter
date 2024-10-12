using MediatR;
using MNX.Application.UseCases;

namespace MNX.MonitoringCenter.Inventory.Contracts.Queries.Drive;

using Drive = Contracts.Drive.Drive;

/// <summary>
/// Запрос на получение списка дисков, работающих на риге.
/// </summary>
public class GetDrivesInfoQuery : IRequest<Result<List<Drive>>>
{
    /// <summary>
    /// Спецификация инвентаризации.
    /// </summary>
    public InventorySpecification Specification { get; }

    public GetDrivesInfoQuery(Guid userId, Guid rigId)
    {
        Specification = new InventorySpecification(userId, rigId);
    }
}
