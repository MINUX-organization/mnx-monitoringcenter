using AutoMapper;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu;

using Gpu = Contracts.Devices.Gpu.Gpu;

/// <summary>
/// Профиль маппинга сущности <see cref="Gpu"/>.
/// </summary>
public class GpusMappingProfile : Profile
{
    public GpusMappingProfile()
    {
        CreateMap<GpuInventoryDto, Gpu>();
        CreateMap<Gpu, GpuInventoryDto>();
    }
}
