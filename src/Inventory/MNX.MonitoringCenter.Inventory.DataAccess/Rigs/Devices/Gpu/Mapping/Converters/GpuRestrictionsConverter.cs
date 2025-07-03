using AutoMapper;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Restrictions;
using MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Enums;
using MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Views;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Mapping.Converters;

/// <summary>
/// Конвертер типа представления ограничений <see cref="GpuRestrictionsView"/>
/// к контракту <see cref="GpuRestrictions"/>.
/// </summary>
public class GpuRestrictionsConverter : ITypeConverter<GpuRestrictionsView, GpuRestrictions>
{
    ///
    public GpuRestrictions Convert(GpuRestrictionsView src, GpuRestrictions dest, ResolutionContext context)
    {
        return src.GetManufacturerEnumValue() switch
        {
            SupportedGpuManufacturerEnum.Nvidia => context.Mapper.Map<NvidiaGpuRestrictions>(src),
            SupportedGpuManufacturerEnum.AMD => context.Mapper.Map<AmdGpuRestrictions>(src),
            SupportedGpuManufacturerEnum.Intel => context.Mapper.Map<IntelGpuRestrictions>(src),
            _ => throw new NotSupportedException($"Unknown GPU manufacturer: {src.Manufacturer}")
        };
    }
}
