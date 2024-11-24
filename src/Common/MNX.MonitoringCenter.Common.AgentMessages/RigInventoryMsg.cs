using MNX.MonitoringCenter.Inventory.Contracts;

namespace MNX.MonitoringCenter.Common.AgentMessages;

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
    /// Идентификатор владельца рига.
    /// </summary>
    public Guid RigOwnerId { get; init; }

    /// <summary>
    /// Дата и время проведения инвентаризации.
    /// </summary>
    public DateTimeOffset CreatedDateTime { get; init; }

    /// <summary>
    /// Инвентаризация.
    /// </summary>
    public required RigInventoryModel Inventory { get; init; }
}