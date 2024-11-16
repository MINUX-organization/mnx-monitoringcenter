namespace MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices;

/// <summary>
/// Устройство.
/// </summary>
public class Device
{
    /// <summary>
    /// Идентификатор видеокарты.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Идентификатор рига.
    /// </summary>
    public Guid RigId { get; }

    /// <summary>
    /// Тип.
    /// </summary>
    public DeviceType Type { get; }

    public Device(Guid id, Guid rigId, DeviceType type)
    {
        Id = id;
        RigId = rigId;
        Type = type;
    }
}
