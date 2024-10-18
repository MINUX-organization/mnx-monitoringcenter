namespace MNX.MonitoringCenter.Inventory.Contracts;

/// <summary>
/// Детали рига.
/// </summary>
public class RigDetails
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Идентификатор владельца.
    /// </summary>
    public Guid OwnerId { get; init; }

    /// <summary>
    /// Название рига.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Программное обеспечение.
    /// </summary>
    public required SoftwareInventory Software { get; init; }
}
