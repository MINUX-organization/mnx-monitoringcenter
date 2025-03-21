using MNX.MonitoringCenter.RigsApi.Contracts.Abstractions;
using MNX.MonitoringCenter.RigsApi.Contracts.Args;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Network;

namespace MNX.MonitoringCenter.RigsApi.Contracts;

public class RigDynamicHardwareIndicatorsModel : IConverterFrom<IEnumerable<RigDynamicHardwareIndicatorsModel>>
{
    /// <summary>
    /// Уникальный идентификатор рига.
    /// </summary>
    public Guid RigId { get; init; }

    /// <summary>
    /// Название рига.
    /// </summary>
    public string? RigName { get; set; }

    /// <summary>
    /// Время работы рига с момента последнего включения.
    /// </summary>
    public int? BootedUpTimeInSeconds { get; init; }

    /// <summary>
    /// Общая мощность.
    /// </summary>
    public int? TotalPower { get; set; }

    /// <summary>
    /// Средняя температура майнинг устройств в градусах Цельсия.
    /// </summary>
    public int? AverageMiningDevicesTemperature { get; set; }

    /// <summary>
    /// Средняя скорость вентиляторов майнинга устройств в процентах.
    /// </summary>
    public int? AverageMiningDevicesFanSpeed { get; set; }

    /// <summary>
    /// Скорость интернета.
    /// </summary>
    public int? InternetSpeed { get; set; }

    /// <summary>
    /// Уровень интернет соединения.
    /// </summary>
    public OnlineState? OnlineState { get; set; }

    public static IEnumerable<RigDynamicHardwareIndicatorsModel>? ConvertFrom<TSource>(TSource source)
    {
        if (source is RigDynamicHardwareIndicatorsModelArgs args)
        {
            return args.RigDynamicHardwareIndicators?.Select(rig => new RigDynamicHardwareIndicatorsModel
            {
                RigId = rig.RigId,
                RigName = args.Rigs.GetValueOrDefault(rig.RigId)?.Name,
                AverageMiningDevicesFanSpeed = rig.AverageMiningDevicesTemperature,
                AverageMiningDevicesTemperature = rig.AverageMiningDevicesTemperature,
                BootedUpTimeInSeconds = rig.BootedUpTimeInSeconds,
                TotalPower = rig.TotalPower,
                InternetSpeed = rig.InternetSpeed,
                OnlineState = rig.OnlineState
            });
        }

        return null;
    }
}
