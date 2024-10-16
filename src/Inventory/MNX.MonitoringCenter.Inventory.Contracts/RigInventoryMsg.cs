namespace MNX.MonitoringCenter.Inventory.Contracts;

/// <summary>
/// Сообщение с инвентаризацией рига.
/// </summary>
public class RigInventoryMsg
{
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
    public required RigInventoryModel Inventory { get; init; }
}
