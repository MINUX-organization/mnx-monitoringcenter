using System.Text.Json.Serialization;
using MNX.MonitoringCenter.Management.Core.FlightSheet.Target;

namespace MNX.MonitoringCenter.Management.Core.Miner.Enums;

/// <summary>
/// Тип поддерживаемого устройства.
/// </summary>
[Flags]
[JsonConverter(typeof(EnumFlagsConverter))]
public enum SupportedDeviceEnum
{
    NvidiaGpu = 1,

    IntelGpu = 1 << 1,

    IntelCpu = 1 << 2,

    AmdGpu = 1 << 3,

    AmdCpu = 1 << 4
}

/// <summary>
/// Расширения для <see cref="SupportedDeviceEnum"/>.
/// </summary>
public static class DeviceEnumExtensions
{
    private static readonly Dictionary<FlightSheetTargetType, List<SupportedDeviceEnum>> TargetToDeviceType = new()
    {
        {
            FlightSheetTargetType.GPU,
            [
                SupportedDeviceEnum.AmdGpu,
                SupportedDeviceEnum.IntelGpu,
                SupportedDeviceEnum.NvidiaGpu
            ]
        },
        {
            FlightSheetTargetType.CPU,
            [
                SupportedDeviceEnum.AmdCpu,
                SupportedDeviceEnum.IntelCpu
            ]
        }
    };

    /// <summary>
    /// Проверка на совместимость типа таргета и устройств.
    /// </summary>
    /// <param name="targetType"> Тип таргета. </param>
    /// <param name="deviceEnum"> Типы устройств. </param>
    /// <returns>
    /// <see langword="true"/>, если тип дивайса поддерживается типом таргета, иначе <see langword="false"/>.
    /// </returns>
    public static bool IsDeviceSupported(this SupportedDeviceEnum deviceEnum, FlightSheetTargetType targetType)
    {
        return TargetToDeviceType[targetType].Any(x => deviceEnum.HasFlag(x));
    }
}