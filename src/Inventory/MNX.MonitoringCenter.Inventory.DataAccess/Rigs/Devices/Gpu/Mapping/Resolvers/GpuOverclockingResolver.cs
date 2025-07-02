using AutoMapper;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Overclocking;
using MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Entities;
using MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Enums;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Mapping.Resolvers;

using Gpu = Contracts.Devices.Gpu.Gpu;

/// <summary>
/// Преобразователь данных разгона видеокарты из сущности <see cref="GpuInventory"/>
/// в контракт <see cref="GpuOverclocking"/>.
/// </summary>
public class GpuOverclockingResolver : IValueResolver<GpuInventory, Gpu, GpuOverclocking>
{
    ///
    public GpuOverclocking Resolve(GpuInventory src, Gpu dest, GpuOverclocking destMember, ResolutionContext context)
    {
        return src.Information.GetManufacturerEnumValue() switch
        {
            SupportedGpuManufacturerEnum.Nvidia => context.Mapper.Map<NvidiaGpuOverclocking>(src.NvidiaOverclocking),
            SupportedGpuManufacturerEnum.AMD => context.Mapper.Map<AmdGpuOverclocking>(src.AmdOverclocking),
            SupportedGpuManufacturerEnum.Intel => context.Mapper.Map<IntelGpuOverclocking>(src.IntelOverclocking),
            _ => throw new NotSupportedException($"Unknown type of gpu manufacturer while mapping")
        };
    }
}
