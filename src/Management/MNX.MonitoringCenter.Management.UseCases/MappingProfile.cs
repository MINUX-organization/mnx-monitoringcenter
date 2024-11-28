using AutoMapper;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.Contracts.FlightSheet;
using MNX.MonitoringCenter.Management.Contracts.FlightSheet.MiningConfigs;
using MNX.MonitoringCenter.Management.Contracts.MiningDevice;
using MNX.MonitoringCenter.Management.Contracts.Presets;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.Core.FlightSheet.Target;
using MNX.MonitoringCenter.Management.Core.Miner.Configs;
using MNX.MonitoringCenter.Management.Core.MiningDevice;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets.EditPreset;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets.SavePreset;
using MNX.MonitoringCenter.Management.UseCases.Cryptocurrency.Commands.AddCryptocurrency;
using MNX.MonitoringCenter.Management.UseCases.FlightSheet.Commands.Models;
using MNX.MonitoringCenter.Management.UseCases.FlightSheet.Commands.Models.MiningConfig;
using MNX.MonitoringCenter.Management.UseCases.Pool.Commands.AddPool;
using MNX.MonitoringCenter.Management.UseCases.Pool.Commands.EditPool;
using MNX.MonitoringCenter.Management.UseCases.Wallet.Commands.AddWallet;
using MNX.MonitoringCenter.Management.UseCases.Wallet.Commands.EditWallet;

namespace MNX.MonitoringCenter.Management.UseCases;

/// <summary>
/// Конфигурация автомаппера
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // cryptocurrency
        
        CreateMap<AddCryptocurrencyCommand, Core.Cryptocurrency>()
            .ForMember(destination => destination.ShortName, options => options.MapFrom(source => source.Model.ShortName))
            .ForMember(destination => destination.FullName, options => options.MapFrom(source => source.Model.FullName))
            .ForMember(destination => destination.AlgorithmId, options => options.MapFrom(source => source.Model.AlgorithmId));

        CreateMap<Core.Cryptocurrency, CryptocurrencyModel>();

        // flight sheets
        
            // request  

        CreateMap<FlightSheetInputModel, Core.FlightSheet.FlightSheet>();
        CreateMap<FlightSheetTargetInputModel, FlightSheetTarget>();

        CreateMap<MiningConfigInputModel, BaseMiningConfig>()
            .Include<CpuMiningConfigInputModel, CpuMiningConfig>()
            .Include<GpuMiningConfigInputModel, GpuMiningConfig>();

        CreateMap<CpuMiningConfigInputModel, CpuMiningConfig>();
        CreateMap<GpuMiningConfigInputModel, GpuMiningConfig>();

        CreateMap<MiningCoinConfigInputModel, MiningCoinConfig>();

            // response

        CreateMap<Core.FlightSheet.FlightSheet, FlightSheetModel>();
        CreateMap<FlightSheetTarget, FlightSheetTargetModel>();

        CreateMap<BaseMiningConfig, BaseMiningConfigModel>()
            .Include<CpuMiningConfig, CpuMiningConfigModel>()
            .Include<GpuMiningConfig, GpuMiningConfigModel>();

        CreateMap<CpuMiningConfig, CpuMiningConfigModel>();
        CreateMap<GpuMiningConfig, GpuMiningConfigModel>();

        CreateMap<MiningCoinConfig, MiningCoinConfigModel>();

        // mining devices

        CreateMap<MiningDeviceInfo, MiningDeviceModel>().ConstructUsing(info => new MiningDeviceModel()
        {
            Id = info.Id,
            RigId = info.RigId,
            Type = info.Type.ToString(),
            FlightSheetName = info.FlightSheet != null ? info.FlightSheet.Name : null,
            FlightSheetIsConfirm = info.FlightSheetIsConfirm,
            MinerName = info.FlightSheet != null
                            ? info.FlightSheet.Targets.First(x => x.DeviceType == info.Type).Miner!.Name
                            : null
        });

        // pools

        CreateMap<Core.Pool, PoolModel>()
            .ForMember(destination => destination.Cryptocurrency, options => options.MapFrom(source => source.Cryptocurrency!.FullName));

        CreateMap<AddPoolCommand, Core.Pool>()
            .ForMember(destination => destination.Port, options => options.MapFrom(source => source.Model.Port))
            .ForMember(destination => destination.Domain, options => options.MapFrom(source => source.Model.Domain))
            .ForMember(destination => destination.CryptocurrencyId, options => options.MapFrom(source => source.Model.CryptocurrencyId));

        CreateMap<EditPoolCommand, Core.Pool>()
            .ForMember(destination => destination.Domain, options => options.MapFrom(source => source.Model.Domain))
            .ForMember(destination => destination.Port, options => options.MapFrom(source => source.Model.Port))
            .ForMember(destination => destination.CryptocurrencyId, options => options.MapFrom(source => source.Model.CryptocurrencyId));

        // presets

        CreateMap<Preset, PresetModel>();

        CreateMap<EditPresetCommand, Preset>()
            .ForMember(destination => destination.Name, options => options.MapFrom(source => source.Model.Name))
            .ForMember(destination => destination.GpuName, options => options.MapFrom(source => source.Model.GpuName))
            .ForMember(destination => destination.Overclocking, options => options.MapFrom(source => source.Model.Overclocking));

        CreateMap<SavePresetCommand, Preset>()
            .ForMember(destination => destination.Name, options => options.MapFrom(source => source.Model.Name))
            .ForMember(destination => destination.GpuName, options => options.MapFrom(source => source.Model.GpuName))
            .ForMember(destination => destination.Overclocking, options => options.MapFrom(source => source.Model.Overclocking));

        CreateMap<OverclockingInputModel, Overclocking>();

        CreateMap<OverclockingModel, Overclocking>().ReverseMap();

        CreateMap<OverclockingModel, OverclockingInputModel>();

        // wallets

        CreateMap<Core.Wallet, WalletModel>()
            .ForMember(destination => destination.Cryptocurrency, options => options.MapFrom(source => source.Cryptocurrency!.FullName));

        CreateMap<AddWalletCommand, Core.Wallet>()
            .ForMember(destination => destination.Name, options => options.MapFrom(source => source.Model.Name))
            .ForMember(destination => destination.Address, options => options.MapFrom(source => source.Model.Address))
            .ForMember(destination => destination.CryptocurrencyId, options => options.MapFrom(source => source.Model.CryptocurrencyId));

        CreateMap<EditWalletCommand, Core.Wallet>()
            .ForMember(destination => destination.Name, options => options.MapFrom(source => source.Model.Name))
            .ForMember(destination => destination.Address, options => options.MapFrom(source => source.Model.Address))
            .ForMember(destination => destination.CryptocurrencyId, options => options.MapFrom(source => source.Model.CryptocurrencyId));
    }
}