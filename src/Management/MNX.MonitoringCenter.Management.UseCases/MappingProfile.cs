using AutoMapper;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.Contracts.FlightSheet;
using MNX.MonitoringCenter.Management.Contracts.FlightSheet.MiningConfigs;
using MNX.MonitoringCenter.Management.Contracts.MiningDevice;
using MNX.MonitoringCenter.Management.Contracts.Overclocking;
using MNX.MonitoringCenter.Management.Contracts.Presets;
using MNX.MonitoringCenter.Management.Core.Mining;
using MNX.MonitoringCenter.Management.Core.Mining.FlightSheet;
using MNX.MonitoringCenter.Management.Core.Mining.FlightSheet.Target;
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Configs;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.UseCases.Mining.Cryptocurrency.Commands.AddCryptocurrency;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.Models;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.Models.MiningConfig;
using MNX.MonitoringCenter.Management.UseCases.Mining.Pool.Commands.AddPool;
using MNX.MonitoringCenter.Management.UseCases.Mining.Pool.Commands.EditPool;
using MNX.MonitoringCenter.Management.UseCases.Mining.Wallet.Commands.AddWallet;
using MNX.MonitoringCenter.Management.UseCases.Mining.Wallet.Commands.EditWallet;
using MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets.Commands.EditPreset;
using MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets.Commands.SavePreset;

namespace MNX.MonitoringCenter.Management.UseCases;

/// <summary>
/// Конфигурация автомаппера
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // cryptocurrency
        
        CreateMap<AddCryptocurrencyCommand, Cryptocurrency>()
            .ForMember(destination => destination.ShortName, options => options.MapFrom(source => source.Model.ShortName))
            .ForMember(destination => destination.FullName, options => options.MapFrom(source => source.Model.FullName))
            .ForMember(destination => destination.AlgorithmId, options => options.MapFrom(source => source.Model.AlgorithmId));

        CreateMap<Cryptocurrency, CryptocurrencyModel>();

        // flight sheets
        
            // request  

        CreateMap<FlightSheetInputModel, FlightSheet>();
        CreateMap<FlightSheetTargetInputModel, FlightSheetTarget>();

        CreateMap<MiningConfigInputModel, BaseMiningConfig>()
            .Include<CpuMiningConfigInputModel, CpuMiningConfig>()
            .Include<GpuMiningConfigInputModel, GpuMiningConfig>();

        CreateMap<CpuMiningConfigInputModel, CpuMiningConfig>();
        CreateMap<GpuMiningConfigInputModel, GpuMiningConfig>();

        CreateMap<MiningCoinConfigInputModel, MiningCoinConfig>();

            // response

        CreateMap<FlightSheet, FlightSheetModel>();
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
            Manufacturer = info.Manufacturer,
            Model = info.Model,
            RigId = info.RigId,
            Type = info.Type.ToString(),
            FlightSheetName = info.FlightSheet != null ? info.FlightSheet.Name : null,
            FlightSheetIsConfirm = info.FlightSheetIsConfirm,
            MinerName = info.FlightSheet != null
                            ? info.FlightSheet.Targets.First(x => x.DeviceType == info.Type).Miner!.Name
                            : null
        });

        // pools

        CreateMap<Pool, PoolModel>()
            .ForMember(destination => destination.Cryptocurrency, options => options.MapFrom(source => source.Cryptocurrency!.FullName));

        CreateMap<AddPoolCommand, Pool>()
            .ForMember(destination => destination.Port, options => options.MapFrom(source => source.Model.Port))
            .ForMember(destination => destination.Domain, options => options.MapFrom(source => source.Model.Domain))
            .ForMember(destination => destination.CryptocurrencyId, options => options.MapFrom(source => source.Model.CryptocurrencyId));

        CreateMap<EditPoolCommand, Pool>()
            .ForMember(destination => destination.Domain, options => options.MapFrom(source => source.Model.Domain))
            .ForMember(destination => destination.Port, options => options.MapFrom(source => source.Model.Port))
            .ForMember(destination => destination.CryptocurrencyId, options => options.MapFrom(source => source.Model.CryptocurrencyId));

        // presets

        CreateMap<Preset, PresetModel>();

        CreateMap<EditPresetCommand, Preset>()
            .ForMember(destination => destination.Name, options => options.MapFrom(source => source.Model.Name))
            .ForMember(destination => destination.DeviceName, options => options.MapFrom(source => source.Model.DeviceName))
            .ForMember(destination => destination.Overclocking, options => options.MapFrom(source => source.Model.Overclocking));

        CreateMap<SavePresetCommand, Preset>()
            .ForMember(destination => destination.Name, options => options.MapFrom(source => source.Model.Name))
            .ForMember(destination => destination.DeviceName, options => options.MapFrom(source => source.Model.DeviceName))
            .ForMember(destination => destination.Overclocking, options => options.MapFrom(source => source.Model.Overclocking));

        CreateMap<Inventory.Contracts.Devices.Gpu.GpuOverclocking, GpuOverclocking>();
        CreateMap<Inventory.Contracts.Devices.Cpu.CpuOverclocking, CpuOverclocking>();

        CreateMap<IOverclocking, IOverclockingModel>()
            .Include<GpuOverclocking, GpuOverclockingModel>()
            .ReverseMap();

        CreateMap<GpuOverclocking, GpuOverclockingModel>().ReverseMap();

        // wallets

        CreateMap<Wallet, WalletModel>()
            .ForMember(destination => destination.Cryptocurrency, options => options.MapFrom(source => source.Cryptocurrency!.FullName));

        CreateMap<AddWalletCommand, Wallet>()
            .ForMember(destination => destination.Name, options => options.MapFrom(source => source.Model.Name))
            .ForMember(destination => destination.Address, options => options.MapFrom(source => source.Model.Address))
            .ForMember(destination => destination.CryptocurrencyId, options => options.MapFrom(source => source.Model.CryptocurrencyId));

        CreateMap<EditWalletCommand, Wallet>()
            .ForMember(destination => destination.Name, options => options.MapFrom(source => source.Model.Name))
            .ForMember(destination => destination.Address, options => options.MapFrom(source => source.Model.Address))
            .ForMember(destination => destination.CryptocurrencyId, options => options.MapFrom(source => source.Model.CryptocurrencyId));
    }
}