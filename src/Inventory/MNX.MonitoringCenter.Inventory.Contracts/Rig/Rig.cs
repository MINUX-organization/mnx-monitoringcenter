namespace MNX.MonitoringCenter.Inventory.Contracts.Rig;

/// <summary>
/// Риг.
/// </summary>
public class Rig
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
}
