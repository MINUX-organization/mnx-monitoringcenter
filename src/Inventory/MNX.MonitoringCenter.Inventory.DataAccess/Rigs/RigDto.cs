namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs;

/// <summary>
/// Риг
/// </summary>
public class RigDto
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
    /// Список инвентаризаций.
    /// </summary>
    public List<RigInventory.RigInventory> Inventories { get; init; } = new(0);

    /// <summary>
    /// Текущая инвентаризация.
    /// </summary>
    public RigInventory.RigInventory? CurrentInventory
    {
        get => Inventories.FirstOrDefault(x => x.IsCurrent);
    }
}
