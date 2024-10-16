using MediatR;

namespace MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs;

/// <summary>
/// Запрос на получение ригов.
/// </summary>
public sealed class GetRigsQuery : IStreamRequest<Contracts.Rig.Rig>
{
    public InventorySpecification Specification { get; }

    public GetRigsQuery(Guid userId, Guid[]? rigsIds = null)
    {
        Specification = new InventorySpecification(userId, rigsIds);
    }
}
