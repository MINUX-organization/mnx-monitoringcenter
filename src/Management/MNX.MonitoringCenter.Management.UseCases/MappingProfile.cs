using AutoMapper;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets.SavePreset;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets.EditPreset;
using MNX.MonitoringCenter.Management.UseCases.Cryptocurrency.Commands;
using MNX.MonitoringCenter.Management.UseCases.Pool.Commands.AddPool;
using MNX.MonitoringCenter.Management.UseCases.Pool.Commands.EditPool;
using MNX.MonitoringCenter.Management.UseCases.Wallet.Commands;
using MNX.MonitoringCenter.Management.UseCases.Wallet.Commands.EditWallet;
using MNX.MonitoringCenter.Management.UseCases.Presets.Commands;

namespace MNX.MonitoringCenter.Management.UseCases;

/// <summary>
/// Конфигурация автомаппера
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CryptocurrencyInputModel, Core.Cryptocurrency>();

        CreateMap<Core.Cryptocurrency, CryptocurrencyModel>();

        CreateMap<Preset, PresetModel>().ReverseMap();

        CreateMap<PresetInputModel, Preset>();

        CreateMap<WalletInputModel, Core.Wallet>();

        CreateMap<OverclockingInputModel, Overclocking>();

        CreateMap<OverclockingModel, Overclocking>().ReverseMap();

        CreateMap<OverclockingModel, OverclockingInputModel>();

        CreateMap<Core.Wallet, WalletModel>()
            .ForMember(destination => destination.Cryptocurrency, options => options.MapFrom(source => source.Cryptocurrency!.FullName));

        CreateMap<Core.Pool, PoolModel>()
            .ForMember(destination => destination.Cryptocurrency, options => options.MapFrom(source => source.Cryptocurrency!.FullName));

        CreateMap<EditPresetCommand, Preset>()
            .ForMember(destination => destination.Name, options => options.MapFrom(source => source.Model.Name))
            .ForMember(destination => destination.GpuName, options => options.MapFrom(source => source.Model.GpuName))
            .ForMember(destination => destination.Overclocking, options => options.MapFrom(source => source.Model.Overclocking));

        CreateMap<SavePresetCommand, Preset>()
            .ForMember(destination => destination.Overclocking, options => options.MapFrom(source => source.Model.Overclocking));

        CreateMap<AddPoolCommand, Core.Pool>()
            .ForMember(destination => destination.Port, options => options.MapFrom(source => source.Model.Port))
            .ForMember(destination => destination.Domain, options => options.MapFrom(source => source.Model.Domain))
            .ForMember(destination => destination.CryptocurrencyId, options => options.MapFrom(source => source.Model.CryptocurrencyId));

        CreateMap<EditPoolCommand, Core.Pool>()
            .ForMember(destination => destination.Domain, options => options.MapFrom(source => source.Model.Domain))
            .ForMember(destination => destination.Port, options => options.MapFrom(source => source.Model.Port))
            .ForMember(destination => destination.Password, options => options.MapFrom(source => source.Model.Password))
            .ForMember(destination => destination.CryptocurrencyId, options => options.MapFrom(source => source.Model.CryptocurrencyId));

        CreateMap<EditWalletCommand, Core.Wallet>()
            .ForMember(destination => destination.Name, options => options.MapFrom(source => source.Model.Name))
            .ForMember(destination => destination.Address, options => options.MapFrom(source => source.Model.Address))
            .ForMember(destination => destination.CryptocurrencyId, options => options.MapFrom(source => source.Model.CryptocurrencyId));             
    }
}