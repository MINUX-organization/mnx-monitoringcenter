using AutoMapper;
using MNX.MonitoringCenter.Management.Core.FlightSheet.Target;
using MNX.MonitoringCenter.Management.Core.Miner.Configs;
using MNX.MonitoringCenter.Management.Core.MiningDevice.Enums;
using MNX.MonitoringCenter.Management.DataAccess.FlightSheet.Dto;
using MNX.MonitoringCenter.Management.DataAccess.FlightSheet.Dto.Target;

namespace MNX.MonitoringCenter.Management.DataAccess;

/// <summary>
/// Конфигурация маппера.
/// </summary>
public class DbMappingProfile : Profile
{
    public DbMappingProfile()
    {
        CreateMap<FlightSheetDto, Core.FlightSheet.FlightSheet>().ReverseMap();

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
}
