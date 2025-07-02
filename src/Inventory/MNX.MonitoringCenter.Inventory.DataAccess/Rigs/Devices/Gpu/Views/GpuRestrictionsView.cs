using MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Entities;
using MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Entities.Restrictions;
using MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Enums;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Views;

/// <summary>
/// Представление ограничений Gpu-устройств, ссылающееся
/// к таблице сущностей <see cref="GpuInventory"/>
/// </summary>
public class GpuRestrictionsView
{
    /// <summary>
    /// Идентификатор видеокарты.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Инвентаризация рига.
    /// </summary>
    public required int RigInventoryId { get; init; }

    /// <summary>
    /// Производитель.
    /// </summary>
    public required string Manufacturer { get; init; }

    /// <summary>
    /// Модель.
    /// </summary>
    public required string Model { get; init; }

    /// <summary>
    /// Ограничения видеокарт Amd.
    /// </summary>
    public required AmdGpuRestrictionsInventory AmdRestrictions { get; init; }

    /// <summary>
    /// Ограничения видеокарт Nvidia.
    /// </summary>
    public required NvidiaGpuRestrictionsInventory NvidiaRestrictions { get; init; }

    /// <summary>
    /// Ограничения видеокарт Intel.
    /// </summary>
    public required IntelGpuRestrictionsInventory IntelRestrictions { get; init; }

    /// <summary>
    /// Получить производителя в качестве значения перечисления.
    /// </summary>
    /// <returns>
    /// Значение перечисления <see cref="SupportedGpuManufacturerEnum"/>.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// Производитель не поддерживается.
    /// </exception>
    public SupportedGpuManufacturerEnum GetManufacturerEnumValue()
    {
        if (Enum.TryParse<SupportedGpuManufacturerEnum>(Manufacturer, ignoreCase: true, out var result))
        {
            return result;
        }
        else
        {
            throw new NotSupportedException("This manufacturer does not support");
        }
    }
}
