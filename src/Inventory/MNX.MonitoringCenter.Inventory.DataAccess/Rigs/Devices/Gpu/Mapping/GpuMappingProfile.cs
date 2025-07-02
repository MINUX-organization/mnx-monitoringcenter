using AutoMapper;
using MNX.MonitoringCenter.Inventory.Contracts.Devices;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Information;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Overclocking;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Restrictions;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Gpu.GetGpusDetails;
using MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Entities;
using MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Entities.Information;
using MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Entities.Overclocking;
using MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Entities.Restrictions;
using MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Mapping.Converters;
using MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Mapping.Resolvers;
using MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Views;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Mapping;

using Gpu = Contracts.Devices.Gpu.Gpu;

/// <summary>
/// Профиль маппинга к <see cref="GpuInventory"/>.
/// </summary>
public class GpuMappingProfile : Profile
{
    ///
    public GpuMappingProfile()
    {
        // General
        CreateMap<Pci, PciInventory>().ReverseMap();

        CreateMap<ParallelComputingTechnology, ParallelComputingTechnologyInventory>().ReverseMap();
        CreateMap<MemoryInformation, MemoryInformationInventory>().ReverseMap();
        CreateMap<GpuInformation, GpuInformationInventory>().ReverseMap();

        CreateMap<NvidiaGpuOverclocking, NvidiaGpuOverclockingInventory>().ReverseMap();
        CreateMap<AmdGpuOverclocking, AmdGpuOverclockingInventory>().ReverseMap();
        CreateMap<IntelGpuOverclocking, IntelGpuOverclockingInventory>().ReverseMap();


        CreateMap<IntegerTypeRestrictions, GpuIntegerTypeRestrictionsInventory>().ReverseMap();
        CreateMap<GpuRestrictions, GpuRestrictionsInventory>().ReverseMap();

        CreateMap<NvidiaGpuRestrictions, NvidiaGpuRestrictionsInventory>()
            .IncludeBase<GpuRestrictions, GpuRestrictionsInventory>().ReverseMap();

        CreateMap<AmdGpuRestrictions, AmdGpuRestrictionsInventory>()
            .IncludeBase<GpuRestrictions, GpuRestrictionsInventory>().ReverseMap();

        CreateMap<IntelGpuRestrictions, IntelGpuRestrictionsInventory>()
            .IncludeBase<GpuRestrictions, GpuRestrictionsInventory>().ReverseMap();

        // To db entities
        CreateMap<Gpu, GpuInventory>()
            .ForMember(dest => dest.AmdOverclocking, opt => opt.MapFrom(src => src.Overclocking as AmdGpuOverclocking))
            .ForMember(dest => dest.NvidiaOverclocking, opt => opt.MapFrom(src => src.Overclocking as NvidiaGpuOverclocking))
            .ForMember(dest => dest.IntelOverclocking, opt => opt.MapFrom(src => src.Overclocking as IntelGpuOverclocking))

            .ForMember(dest => dest.AmdRestrictions, opt => opt.MapFrom(src => src.Restrictions as AmdGpuRestrictions))
            .ForMember(dest => dest.NvidiaRestrictions, opt => opt.MapFrom(src => src.Restrictions as NvidiaGpuRestrictions))
            .ForMember(dest => dest.IntelRestrictions, opt => opt.MapFrom(src => src.Restrictions as IntelGpuRestrictions));

        // To contract entities
        CreateMap<GpuInventoryView, GpuDetails>();

        CreateMap<GpuInventory, Gpu>()
            .ForMember(dest => dest.Restrictions, opt => opt.MapFrom<GpuRestrictionsResolver>())
            .ForMember(dest => dest.Overclocking, opt => opt.MapFrom<GpuOverclockingResolver>());

        CreateMap<GpuRestrictionsView, GpuRestrictions>()
            .ConvertUsing<GpuRestrictionsConverter>();
        CreateMap<GpuRestrictionsView, NvidiaGpuRestrictions>()
            .IncludeBase<GpuRestrictionsView, GpuRestrictions>()
            .IncludeMembers(src => src.NvidiaRestrictions);
        CreateMap<GpuRestrictionsView, AmdGpuRestrictions>()
            .IncludeBase<GpuRestrictionsView, GpuRestrictions>()
            .IncludeMembers(src => src.AmdRestrictions);
        CreateMap<GpuRestrictionsView, IntelGpuRestrictions>()
            .IncludeBase<GpuRestrictionsView, GpuRestrictions>()
            .IncludeMembers(src => src.IntelRestrictions);
    }
}
