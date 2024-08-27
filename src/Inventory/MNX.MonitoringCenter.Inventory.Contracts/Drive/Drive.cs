namespace MNX.MonitoringCenter.Inventory.Contracts.Drive;

/// <summary>
/// Жёсткий диск.
/// </summary>
public class Drive
{
    /// <summary>
    /// Уникальный идентификатор.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Информация.
    /// </summary>
    public DriveInformation Information { get; set; }
}
