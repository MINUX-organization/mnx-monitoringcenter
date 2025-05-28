using AutoMapper;
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Configs;
using MNX.MonitoringCenter.Management.Contracts.FlightSheet.MiningConfigs;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.Models.MiningConfig;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice;

/// <summary>
/// Профиль маппинга для сущностей, связанных с конфигами.
/// </summary>
public class MiningConfigMappingProfile : Profile
{
    public MiningConfigMappingProfile()
    {
        // Base
        CreateMap<BaseMiningConfig, BaseMiningConfigModel>()
            .Include<CpuMiningConfig, CpuMiningConfigModel>()
            .Include<GpuMiningConfig, GpuMiningConfigModel>();

        CreateMap<CpuMiningConfig, CpuMiningConfigModel>();
        CreateMap<GpuMiningConfig, GpuMiningConfigModel>();

        CreateMap<MiningCoinConfig, MiningCoinConfigModel>();

        // Input models
        CreateMap<MiningConfigInputModel, BaseMiningConfig>()
            .Include<CpuMiningConfigInputModel, CpuMiningConfig>()
            .Include<GpuMiningConfigInputModel, GpuMiningConfig>();

        CreateMap<CpuMiningConfigInputModel, CpuMiningConfig>();
        CreateMap<GpuMiningConfigInputModel, GpuMiningConfig>();

        CreateMap<MiningCoinConfigInputModel, MiningCoinConfig>();

        // Coin
        CreateMap<MiningCoinConfig, Agent.Commands.Mining.ApplySettings.Models.MiningCoinConfigModel>()
            .ForMember(destination => destination.WalletAddress, options => options.MapFrom(source => source.Wallet!.Address))
            .ForMember(destination => destination.PoolHost, options => options.MapFrom(source => source.Pool!.Domain))
            .ForMember(destination => destination.PoolPort, options => options.MapFrom(source => source.Pool!.Port))
            .ForMember(destination => destination.PoolPassword, options => options.MapFrom(source => source.PoolPassword));
    }
}
