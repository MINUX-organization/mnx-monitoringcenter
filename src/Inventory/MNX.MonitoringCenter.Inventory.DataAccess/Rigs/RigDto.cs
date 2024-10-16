namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs;

/// <summary>
/// Риг
/// </summary>
internal class RigDto
{
    /// <summary>
    /// Идентификатор рига.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Идентификатор владельца.
    /// </summary>
    public Guid OwnerId { get; init; }

    /// <summary>
    /// Название.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Текущая инвентаризация.
    /// </summary>
    public RigInventory.RigInventory? CurrentInventory { get; set; }
}
