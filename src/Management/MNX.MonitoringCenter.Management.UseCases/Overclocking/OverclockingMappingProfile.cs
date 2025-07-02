using AutoMapper;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.Contracts.Overclocking;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Overclocking;

namespace MNX.MonitoringCenter.Management.UseCases.Overclocking;

using OverclockingInventory = Inventory.Contracts.Devices.Overclocking;

using AmdGpuOverclockingInventory = AmdGpuOverclocking;
using AmdGpuOverclockingCore = Core.Overclocking.Gpu.AmdGpuOverclocking;

using NvidiaGpuOverclockingInventory = NvidiaGpuOverclocking;
using NvidiaGpuOverclockingCore = Core.Overclocking.Gpu.NvidiaGpuOverclocking;

using IntelGpuOverclockingInventory = IntelGpuOverclocking;
using IntelGpuOverclockingCore = Core.Overclocking.Gpu.IntelGpuOverclocking;

using CpuOverclockingInventory = Inventory.Contracts.Devices.Cpu.CpuOverclocking;
using CpuOverclockingCore = CpuOverclocking;

/// <summary>
/// Профиль маппинга для сущностей, связанных с сущностью <see cref="Overclocking"/>.
/// </summary>
public class OverclockingMappingProfile : Profile
{
    ///
    public OverclockingMappingProfile()
    {
        // Inventory to core
        CreateMap<OverclockingInventory, IOverclocking>()
            .ConvertUsing<OverclockingInventoryConverter>();

        CreateMap<AmdGpuOverclockingInventory, AmdGpuOverclockingCore>().ReverseMap();
        CreateMap<NvidiaGpuOverclockingInventory, NvidiaGpuOverclockingCore>().ReverseMap();
        CreateMap<IntelGpuOverclockingInventory, IntelGpuOverclockingCore>().ReverseMap();
        CreateMap<CpuOverclockingInventory, CpuOverclockingCore>().ReverseMap();

        // Core to inventory
        CreateMap<IOverclocking, OverclockingInventory>()
            .Include<CpuOverclockingCore, CpuOverclockingInventory>()
            .Include<AmdGpuOverclockingCore, AmdGpuOverclockingInventory>()
            .Include<NvidiaGpuOverclockingCore, NvidiaGpuOverclockingInventory>()
            .Include<IntelGpuOverclockingCore, IntelGpuOverclockingInventory>()
            .ReverseMap();

        // Core to contract
        CreateMap<IOverclocking, IOverclockingModel>()
            .ConvertUsing<OverclockingModelConverter>();

        CreateMap<AmdGpuOverclockingCore, AmdGpuOverclockingModel>().ReverseMap();
        CreateMap<NvidiaGpuOverclockingCore, NvidiaGpuOverclockingModel>().ReverseMap();
        CreateMap<IntelGpuOverclockingCore, IntelGpuOverclockingModel>().ReverseMap();
        CreateMap<CpuOverclockingCore, CpuOverclockingModel>().ReverseMap();
    }

    ///
    public class OverclockingModelConverter : ITypeConverter<IOverclocking, IOverclockingModel>
    {
        public IOverclockingModel Convert(IOverclocking source, IOverclockingModel destination, ResolutionContext context)
        {
            return source switch
            {
                AmdGpuOverclockingCore amd => context.Mapper.Map<AmdGpuOverclockingModel>(amd),
                NvidiaGpuOverclockingCore nvidia => context.Mapper.Map<NvidiaGpuOverclockingModel>(nvidia),
                IntelGpuOverclockingCore intel => context.Mapper.Map<IntelGpuOverclockingModel>(intel),
                CpuOverclockingCore cpu => context.Mapper.Map<CpuOverclockingModel>(cpu),
                _ => throw new NotSupportedException("Target device type is not supported!")
            };
        }
    }

    ///
    private class OverclockingInventoryConverter : ITypeConverter<OverclockingInventory, IOverclocking>
    {
        public IOverclocking Convert(OverclockingInventory source, IOverclocking destination, ResolutionContext context)
        {
            return source switch
            {
                AmdGpuOverclockingInventory amd => context.Mapper.Map<AmdGpuOverclockingCore>(amd),
                NvidiaGpuOverclockingInventory nvidia => context.Mapper.Map<NvidiaGpuOverclockingCore>(nvidia),
                IntelGpuOverclockingInventory intel => context.Mapper.Map<IntelGpuOverclockingCore>(intel),
                CpuOverclockingInventory cpu => context.Mapper.Map<CpuOverclockingCore>(cpu),
                _ => throw new NotSupportedException("Target device type is not supported!")
            };
        }
    }
}
