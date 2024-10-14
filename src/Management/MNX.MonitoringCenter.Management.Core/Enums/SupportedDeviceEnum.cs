using System.Text.Json.Serialization;
using MNX.MonitoringCenter.Management.Core.FlightSheet.Target;

namespace MNX.MonitoringCenter.Management.Core.Enums;

/// <summary>
/// Тип поддерживаемого устройства.
/// </summary>
[Flags]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SupportedDeviceEnum
{
    NVidiaGpu = 1,

    IntelGpu = 1 << 1,

    IntelCpu = 1 << 2,

    AmdGpu = 1 << 3,

    AmdCpu = 1 << 4
}

/// <summary>
/// Конвертор из <see cref="SupportedDeviceEnum"/> в <see cref="FlightSheetTargetType"/>.
/// </summary>
public static class DeviceEnumConvertor
{
    private static readonly Dictionary<FlightSheetTargetType, List<SupportedDeviceEnum>> TargetToDeviceType = new()
    {
        { 
            FlightSheetTargetType.GPU, 
            [
                SupportedDeviceEnum.AmdGpu, 
                SupportedDeviceEnum.IntelGpu, 
                SupportedDeviceEnum.NVidiaGpu
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
    /// Конвертирует Enum-значение поддерживаемых устройств в список строк.
    /// </summary>
    /// <param name="deviceEnum"> Типы устроств. </param>
    /// <returns> Список строк. </returns>
    public static string[] ToStrings(this SupportedDeviceEnum deviceEnum)
    {
        var maxValue = (int) Enum.GetValues(typeof(SupportedDeviceEnum)).Cast<SupportedDeviceEnum>().Last() * 2; 
        return 0 < (int)deviceEnum && (int)deviceEnum < maxValue
            ? deviceEnum.ToString().Split(", ")
            : [];
    }

    /// <summary>
    /// Проверка на совместимость типа таргета и устройств.
    /// </summary>
    /// <param name="targetType"> Тип таргета. </param>
    /// <param name="deviceEnum"> Типы устроств. </param>
    /// <returns> <see langword="true"/>, если тип дивайса поддерживается типом таргета, иначе <see langword="false"/>. </returns>
    public static bool IsDeviceSupported(this SupportedDeviceEnum deviceEnum, FlightSheetTargetType targetType)
    {
        return TargetToDeviceType[targetType].Any(x => deviceEnum.HasFlag(x));
    }
}