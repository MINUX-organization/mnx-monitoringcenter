using AutoMapper;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Commands.Crypto;
using MNX.MonitoringCenter.Management.UseCases.Commands.Pools.AddPool;
using MNX.MonitoringCenter.Management.UseCases.Commands.Pools.UpdatePool;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets.SavePreset;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets.UpdatePreset;
using MNX.MonitoringCenter.Management.UseCases.Commands.Wallets;
using MNX.MonitoringCenter.Management.UseCases.Commands.Wallets.EditWallet;

namespace MNX.MonitoringCenter.Management.UseCases;

/// <summary>
/// Конфигурация автомаппера
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CryptocurrencyInputModel, Cryptocurrency>();

        CreateMap<Cryptocurrency, CryptocurrencyModel>();

        CreateMap<Preset, PresetModel>();

        CreateMap<PresetModel, Preset>();

        CreateMap<WalletInputModel, Wallet>();

        CreateMap<UpdatePoolCommand, Pool>()
            .ForMember(destination => destination.Domain, options => options.MapFrom(source => source.Model.Domain))
            .ForMember(destination => destination.Port, options => options.MapFrom(source => source.Model.Port))
            .ForMember(destination => destination.CryptocurrencyId, options => options.MapFrom(source => source.Model.CryptocurrencyId));

        CreateMap<EditWalletCommand, Wallet>()
            .ForMember(destination => destination.Name, options => options.MapFrom(source => source.Model.Name))
            .ForMember(destination => destination.Address, options => options.MapFrom(source => source.Model.Address))
            .ForMember(destination => destination.CryptocurrencyId, options => options.MapFrom(source => source.Model.CryptocurrencyId));

        CreateMap<SavePresetCommand, Preset>()
            .ForMember(destination => destination.CoreClock, 
            options => options.MapFrom(source => source.SavePresetModel.PresetModel.CoreClock))
            .ForMember(destination => destination.MemoryClock, 
            options => options.MapFrom(source => source.SavePresetModel.PresetModel.MemoryClock))
            .ForMember(destination => destination.FanSpeed, 
            options => options.MapFrom(source => source.SavePresetModel.PresetModel.FanSpeed))
            .ForMember(destination => destination.PowerLimit, 
            options => options.MapFrom(source => source.SavePresetModel.PresetModel.PowerLimit))
            .ForMember(destination => destination.CriticalTemperature, 
            options => options.MapFrom(source => source.SavePresetModel.PresetModel.CriticalTemperature));

        CreateMap<UpdatePresetCommand, Preset>()
            .ForMember(destination => destination.CoreClock, options => options.MapFrom(source => source.Model.CoreClock))
            .ForMember(destination => destination.MemoryClock, options => options.MapFrom(source => source.Model.MemoryClock))
            .ForMember(destination => destination.FanSpeed, options => options.MapFrom(source => source.Model.FanSpeed))
            .ForMember(destination => destination.PowerLimit, options => options.MapFrom(source => source.Model.PowerLimit))
            .ForMember(destination => destination.CriticalTemperature, options => options.MapFrom(source => source.Model.CriticalTemperature));

        CreateMap<AddPoolCommand, Pool>()
            .ForMember(destination => destination.Port, options => options.MapFrom(source => source.Model.Port))
            .ForMember(destination => destination.Domain, options => options.MapFrom(source => source.Model.Domain))
            .ForMember(destination => destination.CryptocurrencyId, options => options.MapFrom(source => source.Model.CryptocurrencyId));

        CreateMap<Wallet, WalletModel>()
            .ForMember(destination => destination.Cryptocurrency, options => options.MapFrom(source => source.Cryptocurrency!.FullName));
                
        CreateMap<Pool, PoolModel>()
            .ForMember(destination => destination.Cryptocurrency, options => options.MapFrom(source => source.Cryptocurrency!.FullName));
                
    }
}