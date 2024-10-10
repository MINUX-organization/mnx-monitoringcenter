using System.ComponentModel;
using System.Text.Json.Serialization;
using MNX.MonitoringCenter.Management.Core.FlightSheet.Target;

namespace MNX.MonitoringCenter.Management.Core.Enums;

/// <summary>
/// Тип поддерживаемого устройства.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SupportedDeviceEnum
{
    NVidiaGpu,

    IntelGpu,

    IntelCpu,

    AmdGpu,

    AmdCpu
}

/// <summary>
/// Конвертор из <see cref="SupportedDeviceEnum"/> в <see cref="FlightSheetTargetType"/>.
/// </summary>
public static class DeviceTargetTypeConvertor
{
    /// <summary>
    /// Конвертировать из <see cref="SupportedDeviceEnum"/> в <see cref="FlightSheetTargetType"/>.
    /// </summary>
    /// <param name="convertible"> Конвертируемое значение. </param>
    /// <returns> Сконвертированное значение. </returns>
    /// <exception cref="InvalidEnumArgumentException"> Значение не было добавлено в конвертор. </exception>
    public static FlightSheetTargetType Convert(SupportedDeviceEnum convertible)
    {
        return convertible switch
        {
            SupportedDeviceEnum.NVidiaGpu => FlightSheetTargetType.GPU,
            SupportedDeviceEnum.IntelGpu => FlightSheetTargetType.GPU,
            SupportedDeviceEnum.IntelCpu => FlightSheetTargetType.CPU,
            SupportedDeviceEnum.AmdGpu => FlightSheetTargetType.GPU,
            SupportedDeviceEnum.AmdCpu => FlightSheetTargetType.CPU,
            _ => throw new InvalidEnumArgumentException(convertible.ToString(), (int)convertible, typeof(SupportedDeviceEnum))
        };
    }
}