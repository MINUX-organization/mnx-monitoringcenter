using AutoMapper;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.Contracts.Overclocking;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping;

using Overclocking = Inventory.Contracts.Devices.Overclocking;

/// <summary>
/// Профиль маппинга для сущностей, связанных с сущностью <see cref="Overclocking"/>.
/// </summary>
public class OverclockingMappingProfile : Profile
{
    public OverclockingMappingProfile()
    {
        CreateMap<IOverclocking, Overclocking>()
            .Include<CpuOverclocking, Inventory.Contracts.Devices.Cpu.CpuOverclocking>()
            .Include<GpuOverclocking, Inventory.Contracts.Devices.Gpu.GpuOverclocking>()
            .ReverseMap();

        CreateMap<Inventory.Contracts.Devices.Gpu.GpuOverclocking, GpuOverclocking>().ReverseMap();
        CreateMap<Inventory.Contracts.Devices.Cpu.CpuOverclocking, CpuOverclocking>().ReverseMap();

        CreateMap<IOverclocking, IOverclockingModel>()
            .Include<GpuOverclocking, GpuOverclockingModel>()
            .Include<CpuOverclocking, CpuOverclockingModel>()
            .ReverseMap();

        CreateMap<GpuOverclocking, GpuOverclockingModel>().ReverseMap();
        CreateMap<CpuOverclocking, CpuOverclockingModel>().ReverseMap();
    }
}
