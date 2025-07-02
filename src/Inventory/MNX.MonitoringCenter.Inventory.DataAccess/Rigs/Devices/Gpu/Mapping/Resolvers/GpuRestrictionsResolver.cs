using AutoMapper;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Restrictions;
using MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Entities;
using MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Enums;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Mapping.Resolvers;

using Gpu = Contracts.Devices.Gpu.Gpu;

/// <summary>
/// Преобразователь данных ограничений видеокарты из сущности <see cref="GpuInventory"/>
/// в контракт <see cref="GpuRestrictions"/>.
/// </summary>
public class GpuRestrictionsResolver : IValueResolver<GpuInventory, Gpu, GpuRestrictions>
{
    ///
    public GpuRestrictions Resolve(GpuInventory src, Gpu dest, GpuRestrictions destMember, ResolutionContext context)
    {
        return src.Information.GetManufacturerEnumValue() switch
        {
            SupportedGpuManufacturerEnum.Nvidia => context.Mapper.Map<NvidiaGpuRestrictions>(src.NvidiaRestrictions),
            SupportedGpuManufacturerEnum.AMD => context.Mapper.Map<AmdGpuRestrictions>(src.AmdRestrictions),
            SupportedGpuManufacturerEnum.Intel => context.Mapper.Map<IntelGpuRestrictions>(src.IntelRestrictions),
            _ => throw new NotSupportedException($"Unknown type of gpu manufacturer while mapping")
        };
    }
}
