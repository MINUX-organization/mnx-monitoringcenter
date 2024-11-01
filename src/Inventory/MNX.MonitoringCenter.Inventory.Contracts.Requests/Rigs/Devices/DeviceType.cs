namespace MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices;

/// <summary>
/// Тип устройства.
/// </summary>
[Flags]
public enum DeviceType
{
    /// <summary>
    /// Центральный процессор.
    /// </summary>
    CPU = 1 << 0,

    /// <summary>
    /// Диск.
    /// </summary>
    Drive = 1 << 1,

    /// <summary>
    /// Графический процессор.
    /// </summary>
    GPU = 1 << 2,

    /// <summary>
    /// Материнская плата.
    /// </summary>
    Motherboard = 1 << 3,

    /// <summary>
    /// Сетевой адаптер.
    /// </summary>
    NetworkAdapter = 1 << 4
}
