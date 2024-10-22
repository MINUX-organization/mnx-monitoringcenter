 using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Network;
using MNX.MonitoringCenter.Traffic.Observers.Abstractions;
using MNX.MonitoringCenter.Traffic.Observers.Hardware.Contracts.Devices;
using MNX.MonitoringCenter.Traffic.Observers.Hardware.Contracts.Devices.Abstractions;

namespace MNX.MonitoringCenter.Traffic.Observers.Hardware.Contracts;

/// <summary>
/// Динамические аппаратные показатели рига.
/// </summary>
public class RigDynamicHardwareIndicators : IRigIndicators<IDeviceDynamicHardwareIndicators>
{
    /// <summary>
    /// Уникальный идентификатор рига.
    /// </summary>
    public Guid RigId { get; init; }

    /// <summary>
    /// Время работы рига с момента последнего включения.
    /// </summary>
    public DateTime BootedUpTime { get; init; }

    /// <summary>
    /// Общая мощность.
    /// </summary>
    private int? _totalPower;
    public int TotalPower
    {
        get => _totalPower ??= Devices.Sum(device => device.Power);
    }

    /// <summary>
    /// Динамические аппаратные показатели устройств.
    /// </summary>
    public List<IDeviceDynamicHardwareIndicators> Devices { get; init; } = new(0);

    #region MiningDevices
    /// <summary>
    /// Средняя температура майнинг устройств в градусах Цельсия.
    /// </summary>
    private int? _averageMiningDevicesTemperature;
    public int AverageMiningDevicesTemperature
    {
        get
        {
            if (_averageMiningDevicesTemperature is null)
            {
                var averageCpusTemperature = Devices.Where(x => x.Type == DeviceType.CPU)
                                                    .Select(x => (CpuDynamicHardwareIndicators)x)
                                                    .Average(x => x.Temperature);

                var averageGpusTemperature = Devices.Where(x => x.Type == DeviceType.GPU)
                                                    .Select(x => (GpuDynamicHardwareIndicators)x)
                                                    .Average(x => x.GetAverageTemperature());

                _averageMiningDevicesTemperature = (int)((averageCpusTemperature + averageGpusTemperature) / 2);
            }


            return _averageMiningDevicesTemperature.Value; 
        }
    }

    /// <summary>
    /// Средняя скорость вентиляторов майнинга устройств в процентах.
    /// </summary>
    private int? _averageMiningDevicesFanSpeed;
    public int AverageMiningDevicesFanSpeed
    {
        get
        {
            if (_averageMiningDevicesFanSpeed is null)
            {
                var averageCpusFanSpeed = Devices.Where(x => x.Type == DeviceType.CPU)
                                                 .Select(x => (CpuDynamicHardwareIndicators)x)
                                                 .Average(x => x.FanSpeed);

                var averageGpusFanSpeed = Devices.Where(x => x.Type == DeviceType.GPU)
                                                 .Select(x => (GpuDynamicHardwareIndicators)x)
                                                 .Average(x => x.FanSpeed);

                _averageMiningDevicesFanSpeed = (int)((averageCpusFanSpeed + averageGpusFanSpeed) / 2);
            }


            return _averageMiningDevicesFanSpeed.Value;
        }
    }
    #endregion

    #region Network
    /// <summary>
    /// Скорость интернета.
    /// </summary>
    private int? _internetSpeed;
    public int InternetSpeed
    {
        get
        {
            return _internetSpeed ??= Devices.Where(x => x.Type == DeviceType.NetworkAdapter)
                                             .Select(x => (NetworkAdapterDynamicHardwareIndicators)x)
                                             .Single(x => x.IsUse)
                                             .InternetSpeed;
        }
    }

    /// <summary>
    /// Уровень интернет соединения.
    /// </summary>
    private OnlineState? _onlineState;
    public OnlineState OnlineState
    {
        get
        {
            return _onlineState ??= Devices.Where(x => x.Type == DeviceType.NetworkAdapter)
                                           .Select(x => (NetworkAdapterDynamicHardwareIndicators)x)
                                           .Single(x => x.IsUse)
                                           .OnlineState;
        }
    }
    #endregion
}