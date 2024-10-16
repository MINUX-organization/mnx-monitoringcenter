using System.Text.Json.Serialization;
using MNX.MonitoringCenter.Management.Core.FlightSheet.Target;

namespace MNX.MonitoringCenter.Management.Core.Enums;

/// <summary>
/// Тип поддерживаемого устройства.
/// </summary>
[Flags]
[JsonConverter(typeof(EnumFlagsConverter))]
public enum DeviceEnum
{
    NVidiaGpu = 1,

    IntelGpu = 1 << 1,

    IntelCpu = 1 << 2,

    AmdGpu = 1 << 3,

    AmdCpu = 1 << 4
}

/// <summary>
/// Расширения для <see cref="DeviceEnum"/>.
/// </summary>
public static class DeviceEnumExtensions
{
    private static readonly Dictionary<FlightSheetTargetType, List<DeviceEnum>> TargetToDeviceType = new()
    {
        { 
            FlightSheetTargetType.GPU, 
            [
                DeviceEnum.AmdGpu, 
                DeviceEnum.IntelGpu, 
                DeviceEnum.NVidiaGpu
            ]
        },
        {
            FlightSheetTargetType.CPU, 
            [
                DeviceEnum.AmdCpu, 
                DeviceEnum.IntelCpu
            ]
        }
    };

    /// <summary>
    /// Проверка на совместимость типа таргета и устройств.
    /// </summary>
    /// <param name="targetType"> Тип таргета. </param>
    /// <param name="deviceEnum"> Типы устроств. </param>
    /// <returns> <see langword="true"/>, если тип дивайса поддерживается типом таргета, иначе <see langword="false"/>. </returns>
    public static bool IsDeviceSupported(this DeviceEnum deviceEnum, FlightSheetTargetType targetType)
    {
        return TargetToDeviceType[targetType].Any(x => deviceEnum.HasFlag(x));
    }
}