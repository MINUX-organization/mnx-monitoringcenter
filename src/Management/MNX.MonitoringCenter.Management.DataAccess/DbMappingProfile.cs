using AutoMapper;
using MNX.MonitoringCenter.Management.Core.Mining.FlightSheet.Target;
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Configs;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.DataAccess.FlightSheet.Dto;
using MNX.MonitoringCenter.Management.DataAccess.FlightSheet.Dto.Target;
using MNX.MonitoringCenter.Management.DataAccess.Overclocking;

namespace MNX.MonitoringCenter.Management.DataAccess;

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

        CreateMap<IOverclocking, OverclockingDto>()
            .Include<GpuOverclocking, OverclockingDto>()
            .Include<CpuOverclocking, OverclockingDto>();
        CreateMap<GpuOverclocking, OverclockingDto>();
        CreateMap<CpuOverclocking, OverclockingDto>();

        CreateMap<OverclockingDto, IOverclocking>().ConvertUsing(new OverclockingConverter());
        CreateMap<OverclockingDto, CpuOverclocking>();
        CreateMap<OverclockingDto, GpuOverclocking>();
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

    private class OverclockingConverter : ITypeConverter<OverclockingDto, IOverclocking>
    {
        public IOverclocking Convert(OverclockingDto source, IOverclocking destination, ResolutionContext context)
        {
            if (source.TargetDeviceType == OverclockingTargetDeviceType.CPU)
            {
                return context.Mapper.Map<CpuOverclocking>(source);
            }
            else if (source.TargetDeviceType == OverclockingTargetDeviceType.GPU)
            {
                return context.Mapper.Map<GpuOverclocking>(source);
            }

            throw new NotSupportedException("Target device type is not supported!");
        }
    }
}
