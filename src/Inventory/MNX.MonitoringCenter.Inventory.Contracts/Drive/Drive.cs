namespace MNX.MonitoringCenter.Inventory.Contracts.Drive;

/// <summary>
/// Жёсткий диск.
/// </summary>
public record Drive
{
    /// <summary>
    /// Уникальный идентификатор.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Информация.
    /// </summary>
    public required DriveInformation Information { get; init; }
}
