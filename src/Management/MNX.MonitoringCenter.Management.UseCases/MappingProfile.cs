using AutoMapper;
using MNX.MonitoringCenter.Management.Agent.Commands.Mining.ApplySettings.Models;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.Contracts.FlightSheet;
using MNX.MonitoringCenter.Management.Contracts.FlightSheet.MiningConfigs;
using MNX.MonitoringCenter.Management.Contracts.Miner;
using MNX.MonitoringCenter.Management.Contracts.MiningDevice;
using MNX.MonitoringCenter.Management.Contracts.Overclocking;
using MNX.MonitoringCenter.Management.Contracts.Presets;
using MNX.MonitoringCenter.Management.Core.Mining;
using MNX.MonitoringCenter.Management.Core.Mining.FlightSheet;
using MNX.MonitoringCenter.Management.Core.Mining.FlightSheet.Target;
using MNX.MonitoringCenter.Management.Core.Mining.Miner;
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Configs;
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Enums;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.UseCases.Mining.Cryptocurrency.Commands.AddCryptocurrency;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.Models;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.Models.MiningConfig;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.Models;
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

        CreateMap<MiningCoinConfig, Contracts.FlightSheet.MiningConfigs.MiningCoinConfigModel>();

        // miners

        CreateMap<MinerInputModel, Miner>()
            .ForMember(miner => miner.Type, options => options.MapFrom(_ => MinerTypeEnum.Custom));
        CreateMap<Miner, MinerModel>();
        CreateMap<MinerAlgorithm, MinerAlgorithmModel>()
            .ConstructUsing(algo => new MinerAlgorithmModel(algo.AlgorithmId, algo.Name));

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

        CreateMap<DeviceFLightSheet, WorkerSettings>().ConstructUsing((x, c) => new WorkerSettings()
        {
            WorkerId = x.Device.Id,
            SettingsModel = c.Mapper.Map<BaseMiningSettingsModel>(x.FlightSheet)
        });

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

        CreateMap<MiningCoinConfig, Agent.Commands.Mining.ApplySettings.Models.MiningCoinConfigModel>()
            .ForMember(destination => destination.WalletAddress, options => options.MapFrom(source => source.Wallet!.Address))
            .ForMember(destination => destination.PoolHost, options => options.MapFrom(source => source.Pool!.Domain))
            .ForMember(destination => destination.PoolPort, options => options.MapFrom(source => source.Pool!.Port))
            .ForMember(destination => destination.PoolPassword, options => options.MapFrom(source => source.PoolPassword));

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
            .ForMember(destination => destination.Overclocking, options => options.MapFrom(source => source.Model.Overclocking))
            .AfterMap((command, preset) => preset.OverclockingId = preset.Overclocking!.Id);

        CreateMap<SavePresetCommand, Preset>()
            .ForMember(destination => destination.Name, options => options.MapFrom(source => source.Model.Name))
            .ForMember(destination => destination.DeviceName, options => options.MapFrom(source => source.Model.DeviceName))
            .ForMember(destination => destination.Overclocking, options => options.MapFrom(source => source.Model.Overclocking))
            .AfterMap((command, preset) => preset.OverclockingId = preset.Overclocking!.Id);

        CreateMap<IOverclocking, Inventory.Contracts.Devices.Overclocking>()
            .Include<CpuOverclocking, Inventory.Contracts.Devices.Cpu.CpuOverclocking>()
            .Include<GpuOverclocking, Inventory.Contracts.Devices.Gpu.GpuOverclocking>()
            .ReverseMap();

        CreateMap<Inventory.Contracts.Devices.Gpu.GpuOverclocking, GpuOverclocking>().ReverseMap();
        CreateMap<Inventory.Contracts.Devices.Cpu.CpuOverclocking, CpuOverclocking>().ReverseMap();

        CreateMap<IOverclocking, IOverclockingModel>()
            .Include<GpuOverclocking, GpuOverclockingModel>()
            .Include<CpuOverclocking, CpuOverclockingModel>()
            .ReverseMap();

        CreateMap<GpuOverclocking, GpuOverclockingModel>().ReverseMap();
        CreateMap<CpuOverclocking, CpuOverclockingModel>().ReverseMap();

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
        List<Agent.Commands.Mining.ApplySettings.Models.MiningCoinConfigModel> miningCoinConfigs,
        FlightSheetTarget flightSheetTarget)
    {
        for (int i = 0; i < miningCoinConfigs.Count; i++)
        {
            var algorithmId = flightSheetTarget.MiningConfig.CoinConfigs[i].Wallet!.Cryptocurrency!.AlgorithmId;
            miningCoinConfigs[i].AlgorithmName = flightSheetTarget.Miner!.SupportedAlgorithms.First(algo => algo.AlgorithmId == algorithmId).Name;
        }
    }
}