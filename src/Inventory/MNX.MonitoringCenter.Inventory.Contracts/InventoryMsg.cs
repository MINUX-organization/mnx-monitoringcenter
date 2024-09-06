namespace MNX.MonitoringCenter.Inventory.Contracts;

/// <summary>
/// Сообщение с инвентаризацией.
/// </summary>
public class InventoryMsg
{
    /// <summary>
    /// Идентификатор владельца рига.
    /// </summary>
    public Guid RigOwnerId { get; init; }

    /// <summary>
    /// Идентификатор рига.
    /// </summary>
    public Guid RigId { get; init; }

    /// <summary>
    /// Дата и время проведения инвентаризации.
    /// </summary>
    public DateTimeOffset CreatedDateTime { get; init; }

    /// <summary>
    /// Инвентаризация.
    /// </summary>
    public required InventoryModel Inventory { get; init; }
}
