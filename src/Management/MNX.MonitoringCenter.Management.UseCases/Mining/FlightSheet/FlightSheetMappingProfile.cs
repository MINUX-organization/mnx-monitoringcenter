using AutoMapper;
using MNX.MonitoringCenter.Management.Contracts.FlightSheet;
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Configs;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;
using MNX.MonitoringCenter.Management.Core.Mining.FlightSheet.Target;
using MNX.MonitoringCenter.Management.Agent.Commands.Mining.ApplySettings.Models;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.Models;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet;

using FlightSheet = Core.Mining.FlightSheet.FlightSheet;

/// <summary>
/// Профиль маппинга для сущностей, связанных с сущностью <see cref="FlightSheet"/>.
/// </summary>
public class FlightSheetMappingProfile : Profile
{
    public FlightSheetMappingProfile()
    {
        CreateMap<FlightSheetInputModel, FlightSheet>();
        CreateMap<FlightSheetTargetInputModel, FlightSheetTarget>();

        CreateMap<FlightSheet, FlightSheetModel>();
        CreateMap<FlightSheetTarget, FlightSheetTargetModel>();

        CreateMap<FlightSheetTarget, BaseMiningSettingsModel>().ConvertUsing(new FlightSheetTargetToMiningSettingsModelConverter());

        CreateMap<FlightSheetTarget, GpuMiningSettingsModel>()
            .ForMember(destination => destination.MinerName, options => options.MapFrom(source => source.Miner!.Name))
            .ForMember(destination => destination.MinerVersion, options => options.MapFrom(source => source.Miner!.Version))
            .ForMember(destination => destination.CoinConfigs, options => options.MapFrom(source => source.MiningConfig.CoinConfigs))
            .ForMember(destination => destination.AdditionalArguments, options => options.MapFrom(source => source.MiningConfig.AdditionalArguments))
            .ForMember(destination => destination.ConfigFileContent, options => options.MapFrom(source => source.MiningConfig.ConfigFileContent))
            .AfterMap((flightSheetTarget, settings) => SetAlgorithmName(settings.CoinConfigs, flightSheetTarget));

        CreateMap<FlightSheetTarget, CpuMiningSettingsModel>()
            .ForMember(destination => destination.MinerName, options => options.MapFrom(source => source.Miner!.Name))
            .ForMember(destination => destination.MinerVersion, options => options.MapFrom(source => source.Miner!.Version))
            .ForMember(destination => destination.CoinConfigs, options => options.MapFrom(source => source.MiningConfig.CoinConfigs))
            .ForMember(destination => destination.AdditionalArguments, options => options.MapFrom(source => source.MiningConfig.AdditionalArguments))
            .ForMember(destination => destination.ConfigFileContent, options => options.MapFrom(source => source.MiningConfig.ConfigFileContent))
            .ForMember(destination => destination.HugePages, options => options.MapFrom(source => ((CpuMiningConfig)source.MiningConfig).HugePages))
            .ForMember(destination => destination.ThreadsCount, options => options.MapFrom(source => ((CpuMiningConfig)source.MiningConfig).ThreadsCount))
            .AfterMap((flightSheetTarget, settings) => SetAlgorithmName(settings.CoinConfigs, flightSheetTarget));
    }

    private class FlightSheetTargetToMiningSettingsModelConverter : ITypeConverter<FlightSheetTarget, BaseMiningSettingsModel>
    {
        public BaseMiningSettingsModel Convert(FlightSheetTarget source, BaseMiningSettingsModel destination, ResolutionContext context)
        {
            if (source.DeviceType == MiningDeviceType.CPU)
            {
                return context.Mapper.Map<CpuMiningSettingsModel>(source);
            }
            else if (source.DeviceType == MiningDeviceType.GPU)
            {
                return context.Mapper.Map<GpuMiningSettingsModel>(source);
            }

            throw new NotImplementedException("Mining device type is not supported!");
        }
    }

    private static void SetAlgorithmName(
        List<MiningCoinConfigModel> miningCoinConfigs,
        FlightSheetTarget flightSheetTarget)
    {
        for (int i = 0; i < miningCoinConfigs.Count; i++)
        {
            var algorithmId = flightSheetTarget.MiningConfig.CoinConfigs[i].Wallet!.Cryptocurrency!.AlgorithmId;
            miningCoinConfigs[i].AlgorithmName = flightSheetTarget.Miner!.SupportedAlgorithms.First(algo => algo.AlgorithmId == algorithmId).Name;
        }
    }
}
