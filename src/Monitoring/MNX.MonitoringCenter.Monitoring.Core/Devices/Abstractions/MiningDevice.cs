using MNX.MonitoringCenter.Monitoring.Core.Devices.Enums;
using MNX.MonitoringCenter.Monitoring.Core.Devices.Gpu;

namespace MNX.MonitoringCenter.Monitoring.Core.Devices.Abstractions;

/// <summary>
/// Майнинг устройство.
/// </summary>
public abstract class MiningDevice
{
    /// <summary>
    /// Уникальный идентификатор.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Название.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Название рига.
    /// </summary>
    public string RigName { get; init; }

    /// <summary>
    /// Серийный номер.
    /// </summary>
    public string SerialNumber { get; init; }

    /// <summary>
    /// Тип.
    /// </summary>
    public abstract MiningDeviceType Type { get; }

    /// <summary>
    /// Производитель.
    /// </summary>
    public string Manufacturer { get; }

    /// <summary>
    /// Разгон.
    /// </summary>
    public GpuOverclocking? Overclocking { get; set; }

    /// <summary>
    /// Майнер.
    /// </summary>
    public string? Miner { get; set; }

    public MiningDevice(Guid id, string name, string rigName, string manufacturer, string serialNumber)
    {
        Id = id;
        Name = name;
        RigName = rigName;
        Manufacturer = manufacturer;
        SerialNumber = serialNumber;
    }
}
