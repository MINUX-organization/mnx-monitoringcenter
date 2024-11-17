using MediatR;

namespace MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs;

/// <summary>
/// Запрос на получения списка ригов.
/// </summary>
public sealed class GetRigsQuery : IStreamRequest<Rig>
{
    /// <summary>
    /// Спецификация для инвентаризации.
    /// </summary>
    public InventorySpecification Specification { get; }

    public GetRigsQuery(Guid userId, Guid[]? rigsIds = null)
    {
        Specification = new InventorySpecification(userId, rigsIds);
    }
}
