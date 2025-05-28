using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;

/// <summary>
/// Перечисление комбинаций производителей и типов устройств.
/// </summary>
[Flags]
[JsonConverter(typeof(EnumFlagsConverter))]
public enum DeviceTypeManufacturerCombination
{
    None = 0,

    NvidiaGpu = 1, // 1

    IntelGpu = 1 << 1, // 2

    IntelCpu = 1 << 2, // 4

    AmdGpu = 1 << 3, // 8

    AmdCpu = 1 << 4 // 16
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

    private static readonly Dictionary<string, List<DeviceTypeManufacturerCombination>>
        DeviceManufacturerToTypeCombination = new()
        {
            {
                MiningDeviceManufacturer.AMD.ToString(),
                [
                    DeviceTypeManufacturerCombination.AmdGpu,
                    DeviceTypeManufacturerCombination.AmdCpu
                ]
            },
            {
                MiningDeviceManufacturer.Nvidia.ToString(),
                [
                    DeviceTypeManufacturerCombination.NvidiaGpu
                ]
            },
            {
                MiningDeviceManufacturer.Intel.ToString(),
                [
                    DeviceTypeManufacturerCombination.IntelGpu,
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

    /// <summary>
    /// Проверка на совместимость производителя устройства и комбинаций производителей и типов устройств.
    /// </summary>
    /// <param name="deviceEnum"> Комбинации производителей и типов устройств. </param>
    /// <param name="manufacturer"> Производитель. </param>
    /// <returns>
    /// <see langword="true"/>, если комбинация производителя и устройства содержит в себе переданный тип устройства,
    /// иначе <see langword="false"/>.
    /// </returns>
    public static bool IsSupportDeviceManufacturer(this DeviceTypeManufacturerCombination deviceEnum,
                                                   string manufacturer)
    {
        if (DeviceManufacturerToTypeCombination.TryGetValue(manufacturer, out List<DeviceTypeManufacturerCombination>? list))
        {
            return list.Any(x => deviceEnum.HasFlag(x));
        };

        return false;
    }
}