using AutoMapper;
using MNX.MonitoringCenter.Management.Core.Mining.FlightSheet.Target;
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Configs;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.Core.Overclocking.Cpu;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu.Fan;
using MNX.MonitoringCenter.Management.DataAccess.FlightSheet.Dto;
using MNX.MonitoringCenter.Management.DataAccess.FlightSheet.Dto.Target;
using MNX.MonitoringCenter.Management.DataAccess.Mapping.Converters;
using MNX.MonitoringCenter.Management.DataAccess.Overclocking;

namespace MNX.MonitoringCenter.Management.DataAccess.Mapping;

using AmdGpuOverclockingCore = Core.Overclocking.Gpu.AmdGpuOverclocking;
using CpuOverclockingCore = CpuOverclocking;
using IntelGpuOverclockingCore = Core.Overclocking.Gpu.IntelGpuOverclocking;
using NvidiaGpuOverclockingCore = Core.Overclocking.Gpu.NvidiaGpuOverclocking;

/// <summary>
/// Конфигурация маппера.
/// </summary>
public partial class DbMappingProfile : Profile
{
    ///
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

        CreateMap<FanOverclockingDto, FanOverclockingDto>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

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

        CreateMap<FanOverclockingWithTargetSpeed, FanOverclockingDto>();
        CreateMap<FanOverclockingWithTargetTemperature, FanOverclockingDto>();
        CreateMap<FanOverclockingWithLinearDependence, FanOverclockingDto>();

        // Db to core
        CreateMap<OverclockingDto, IOverclocking>().ConvertUsing(new OverclockingDtoConverter());
        CreateMap<OverclockingDto, CpuOverclockingCore>();
        CreateMap<OverclockingDto, AmdGpuOverclockingCore>()
            .ForMember(dest => dest.FanOverclocking, opt => opt.MapFrom(src => src.FanOverclocking));
        CreateMap<OverclockingDto, NvidiaGpuOverclockingCore>()
            .ForMember(dest => dest.FanOverclocking, opt => opt.MapFrom(src => src.FanOverclocking));
        CreateMap<OverclockingDto, IntelGpuOverclockingCore>();

        CreateMap<FanOverclockingDto, IFanOverclocking>()
            .ConvertUsing<FanOverclockingDtoConverter>();
        CreateMap<FanOverclockingDto, FanOverclockingWithTargetSpeed>();
        CreateMap<FanOverclockingDto, FanOverclockingWithTargetTemperature>();
        CreateMap<FanOverclockingDto, FanOverclockingWithLinearDependence>();
    }
}
