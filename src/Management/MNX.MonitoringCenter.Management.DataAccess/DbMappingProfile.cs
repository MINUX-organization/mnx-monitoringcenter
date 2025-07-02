using AutoMapper;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.DataAccess.Overclocking;
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Configs;
using MNX.MonitoringCenter.Management.DataAccess.FlightSheet.Dto;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;
using MNX.MonitoringCenter.Management.Core.Mining.FlightSheet.Target;
using MNX.MonitoringCenter.Management.DataAccess.FlightSheet.Dto.Target;

namespace MNX.MonitoringCenter.Management.DataAccess;

using AmdGpuOverclockingCore = Core.Overclocking.Gpu.AmdGpuOverclocking;

using NvidiaGpuOverclockingCore = Core.Overclocking.Gpu.NvidiaGpuOverclocking;

using IntelGpuOverclockingCore = Core.Overclocking.Gpu.IntelGpuOverclocking;

using CpuOverclockingCore = CpuOverclocking;

/// <summary>
/// Конфигурация маппера.
/// </summary>
public class DbMappingProfile : Profile
{
    public DbMappingProfile()
    {
        CreateMap<FlightSheetDto, Core.Mining.FlightSheet.FlightSheet>().ReverseMap();

        CreateMap<BaseFlightSheetTargetDto, FlightSheetTarget>()
            .ForMember(dest => dest.DeviceType, member => member.Ignore())
            .ForMember(dest => dest.MiningConfig, member => member.MapFrom(dto => dto.CreateMiningConfig()));

        CreateMap<FlightSheetTarget, BaseFlightSheetTargetDto>().ConvertUsing(new FlightSheetTargetConverter());

        CreateMap<FlightSheetTarget, CpuFlightSheetTargetDto>()
            .ForMember(dest => dest.CoinConfigs, opt => opt.MapFrom(x => x.MiningConfig.CoinConfigs))
            .ForMember(dest => dest.AdditionalArguments, opt => opt.MapFrom(x => x.MiningConfig.AdditionalArguments))
            .ForMember(dest => dest.ConfigFileContent, opt => opt.MapFrom(x => x.MiningConfig.ConfigFileContent))
            .ForMember(dest => dest.HugePages, opt => opt.MapFrom(x => ((CpuMiningConfig)x.MiningConfig).HugePages))
            .ForMember(dest => dest.ThreadsCount, opt => opt.MapFrom(x => ((CpuMiningConfig)x.MiningConfig).ThreadsCount));

        CreateMap<FlightSheetTarget, GpuFlightSheetTargetDto>()
            .ForMember(dest => dest.CoinConfigs, opt => opt.MapFrom(x => x.MiningConfig.CoinConfigs))
            .ForMember(dest => dest.AdditionalArguments, opt => opt.MapFrom(x => x.MiningConfig.AdditionalArguments))
            .ForMember(dest => dest.ConfigFileContent, opt => opt.MapFrom(x => x.MiningConfig.ConfigFileContent));


        // Overclocking

        // Db to db
        CreateMap<OverclockingDto, OverclockingDto>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.TargetDeviceType, opt => opt.Ignore());


        // Core to db
        CreateMap<IOverclocking, OverclockingDto>()
            .Include<AmdGpuOverclockingCore, OverclockingDto>()
            .Include<NvidiaGpuOverclockingCore, OverclockingDto>()
            .Include<IntelGpuOverclockingCore, OverclockingDto>()
            .Include<CpuOverclockingCore, OverclockingDto>();

        CreateMap<AmdGpuOverclockingCore, OverclockingDto>();
        CreateMap<NvidiaGpuOverclockingCore, OverclockingDto>();
        CreateMap<IntelGpuOverclockingCore, OverclockingDto>();
        CreateMap<CpuOverclockingCore, OverclockingDto>();


        // Db to core
        CreateMap<OverclockingDto, IOverclocking>().ConvertUsing(new OverclockingDtoConverter());
        CreateMap<OverclockingDto, CpuOverclockingCore>();
        CreateMap<OverclockingDto, AmdGpuOverclockingCore>();
        CreateMap<OverclockingDto, NvidiaGpuOverclockingCore>();
        CreateMap<OverclockingDto, IntelGpuOverclockingCore>();
    }

    private class FlightSheetTargetConverter : ITypeConverter<FlightSheetTarget, BaseFlightSheetTargetDto>
    {
        public BaseFlightSheetTargetDto Convert(FlightSheetTarget source, BaseFlightSheetTargetDto destination, ResolutionContext context)
        {
            if (source.DeviceType == MiningDeviceType.CPU)
            {
                return context.Mapper.Map<CpuFlightSheetTargetDto>(source);
            }
            else if (source.DeviceType == MiningDeviceType.GPU)
            {
                return context.Mapper.Map<GpuFlightSheetTargetDto>(source);
            }

            throw new NotSupportedException("Mining device type is not supported!");
        }
    }

    private class OverclockingDtoConverter : ITypeConverter<OverclockingDto, IOverclocking>
    {
        public IOverclocking Convert(OverclockingDto source, IOverclocking destination, ResolutionContext context)
        {
            return source.TargetDeviceType switch
            {
                OverclockingTargetDeviceType.CPU => context.Mapper.Map<CpuOverclockingCore>(source),
                OverclockingTargetDeviceType.AmdGPU => context.Mapper.Map<AmdGpuOverclockingCore>(source),
                OverclockingTargetDeviceType.NvidiaGPU => context.Mapper.Map<NvidiaGpuOverclockingCore>(source),
                OverclockingTargetDeviceType.IntelGPU => context.Mapper.Map<IntelGpuOverclockingCore>(source),
                _ => throw new NotSupportedException("Target device type is not supported!")
            };
        }
    }
}
