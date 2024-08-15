using AutoMapper;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets.SavePreset;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets.UpdatePreset;
using MNX.MonitoringCenter.Management.UseCases.Cryptocurrency.Commands;
using MNX.MonitoringCenter.Management.UseCases.Pool.Commands.AddPool;
using MNX.MonitoringCenter.Management.UseCases.Pool.Commands.UpdatePool;
using MNX.MonitoringCenter.Management.UseCases.Wallet.Commands;
using MNX.MonitoringCenter.Management.UseCases.Wallet.Commands.EditWallet;

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

        CreateMap<Preset, PresetModel>().ReverseMap();

        CreateMap<SavePresetInputModel, Preset>();

        CreateMap<WalletInputModel, Wallet>();

        CreateMap<OverclockingInputModel, Overclocking>();

        CreateMap<OverclockingModel, Overclocking>().ReverseMap();

        CreateMap<OverclockingModel, OverclockingInputModel>();

        CreateMap<Wallet, WalletModel>()
            .ForMember(destination => destination.Cryptocurrency, options => options.MapFrom(source => source.Cryptocurrency!.FullName));

        CreateMap<Pool, PoolModel>()
            .ForMember(destination => destination.Cryptocurrency, options => options.MapFrom(source => source.Cryptocurrency!.FullName));

        CreateMap<UpdatePresetCommand, Preset>()
            .ForMember(destination => destination.Name, options => options.MapFrom(source => source.SavePresetModel.Name))
            .ForMember(destination => destination.GpuName, options => options.MapFrom(source => source.SavePresetModel.GpuName))
            .ForMember(destination => destination.Overclocking, options => options.MapFrom(source => source.SavePresetModel.Overclocking));

        CreateMap<SavePresetCommand, Preset>()
            .ForMember(destination => destination.Overclocking, options => options.MapFrom(source => source.SavePresetModel.Overclocking));

        CreateMap<AddPoolCommand, Pool>()
            .ForMember(destination => destination.Port, options => options.MapFrom(source => source.Model.Port))
            .ForMember(destination => destination.Domain, options => options.MapFrom(source => source.Model.Domain))
            .ForMember(destination => destination.CryptocurrencyId, options => options.MapFrom(source => source.Model.CryptocurrencyId));

        CreateMap<UpdatePoolCommand, Pool>()
            .ForMember(destination => destination.Domain, options => options.MapFrom(source => source.Model.Domain))
            .ForMember(destination => destination.Port, options => options.MapFrom(source => source.Model.Port))
            .ForMember(destination => destination.CryptocurrencyId, options => options.MapFrom(source => source.Model.CryptocurrencyId));

        CreateMap<EditWalletCommand, Wallet>()
            .ForMember(destination => destination.Name, options => options.MapFrom(source => source.Model.Name))
            .ForMember(destination => destination.Address, options => options.MapFrom(source => source.Model.Address))
            .ForMember(destination => destination.CryptocurrencyId, options => options.MapFrom(source => source.Model.CryptocurrencyId));             
    }
}