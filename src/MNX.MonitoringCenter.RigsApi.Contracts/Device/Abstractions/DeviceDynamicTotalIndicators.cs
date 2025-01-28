using MNX.MonitoringCenter.RigsApi.Contracts.FlightSheet;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Mining;

namespace MNX.MonitoringCenter.RigsApi.Contracts.Device.Abstractions;

/// <summary>
/// Динамические аппаратные показатели устройства.
/// </summary>
public abstract class DeviceDynamicTotalIndicators
{
    /// <summary>
    /// Идентификатор устройства.
    /// </summary>
    public Guid DeviceId { get; init; }

    /// <summary>
    /// Тип устройства.
    /// </summary>
    public abstract DeviceType Type { get; }

    /// <summary>
    /// Имя устройства.
    /// </summary>
    public string? DeviceName { get; init; }

    /// <summary>
    /// Мощность устройства.
    /// </summary>
    public int Power { get; set; }

    /// <summary>
    /// Статистика полетных листов.
    /// </summary>
    public FlightSheetStatisticsModel? FlightSheet { get; set; }
    
    /// <summary>
    /// Состояние майнинга.
    /// </summary>
    public MiningState MiningState { get; set; }
}