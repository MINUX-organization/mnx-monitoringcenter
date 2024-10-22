using MediatR;

namespace MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs;

/// <summary>
/// Запрос на получение детального описания ригов.
/// </summary>
public sealed class GetRigsDetailsQuery : IStreamRequest<RigDetails>
{
    public InventorySpecification Specification { get; }

    public GetRigsDetailsQuery(Guid userId, Guid[]? rigsIds = null)
    {
        Specification = new InventorySpecification(userId, rigsIds);
    }
}
