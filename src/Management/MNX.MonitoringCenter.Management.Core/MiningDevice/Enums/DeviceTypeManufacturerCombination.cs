using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Management.Core.MiningDevice.Enums;

/// <summary>
/// Перечисление комбинаций производителей и типов устройств.
/// </summary>
[Flags]
[JsonConverter(typeof(EnumFlagsConverter))]
public enum DeviceTypeManufacturerCombination
{
    NvidiaGpu = 1,

    IntelGpu = 1 << 1,

    IntelCpu = 1 << 2,

    AmdGpu = 1 << 3,

    AmdCpu = 1 << 4
}

/// <summary>
/// Расширения для <see cref="DeviceTypeManufacturerCombination"/>.
/// </summary>
public static class DeviceEnumExtensions
{
    private static readonly Dictionary<MiningDeviceType, List<DeviceTypeManufacturerCombination>>
        DeviceTypeToTypeManufacturerCombination = new()
    {
        {
            MiningDeviceType.GPU,
            [
                DeviceTypeManufacturerCombination.AmdGpu,
                DeviceTypeManufacturerCombination.IntelGpu,
                DeviceTypeManufacturerCombination.NvidiaGpu
            ]
        },
        {
            MiningDeviceType.CPU,
            [
                DeviceTypeManufacturerCombination.AmdCpu,
                DeviceTypeManufacturerCombination.IntelCpu
            ]
        }
    };

    /// <summary>
    /// Проверка на совместимость типа устройства и комбинаций производителей и типов устройств.
    /// </summary>
    /// <param name="deviceType"> Тип устройства. </param>
    /// <param name="deviceEnum"> Комбинации производителей и типов устройств. </param>
    /// <returns>
    /// <see langword="true"/>, если комбинация производителя и устройства содержит в себе переданный тип устройства,
    /// иначе <see langword="false"/>.
    /// </returns>
    public static bool IsSupportDeviceType(this DeviceTypeManufacturerCombination deviceEnum,
                                           MiningDeviceType deviceType)
    {
        return DeviceTypeToTypeManufacturerCombination[deviceType].Any(x => deviceEnum.HasFlag(x));
    }
}