using AutoMapper;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.Contracts.FlightSheet;
using MNX.MonitoringCenter.Management.Contracts.FlightSheet.Target;
using MNX.MonitoringCenter.Management.Contracts.Presets;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.Core.FlightSheet.Target;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets.EditPreset;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets.SavePreset;
using MNX.MonitoringCenter.Management.UseCases.Cryptocurrency.Commands.AddCryptocurrency;
using MNX.MonitoringCenter.Management.UseCases.FlightSheet.Commands.Converters;
using MNX.MonitoringCenter.Management.UseCases.FlightSheet.Commands.Models;
using MNX.MonitoringCenter.Management.UseCases.FlightSheet.Commands.Models.Target;
using MNX.MonitoringCenter.Management.UseCases.FlightSheet.Queries.Models;
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
        
        CreateMap<FlightSheetInputModel, Core.FlightSheet.FlightSheet>();
        CreateMap<Core.FlightSheet.FlightSheet, FlightSheetModel>();

        CreateMap<Core.FlightSheet.FlightSheet, FlightSheetOutputModel>();
        CreateMap<FlightSheetTargetBase, FlightSheetTargetOutputModel>();

        CreateMap<FlightSheetTargetBase, FlightSheetTargetModelBase>().ConvertUsing(new FlightSheetTargetConverter());
        CreateMap<CpuFlightSheetTarget, CpuFlightSheetTargetModel>();
        CreateMap<GpuFlightSheetTarget, GpuFlightSheetTargetModel>();

        CreateMap<FlightSheetTargetInputModel, FlightSheetTargetBase>().ConvertUsing(new FlightSheetTargetInputModelConverter());
        CreateMap<CpuFlightSheetTargetInputModel, CpuFlightSheetTarget>();
        CreateMap<GpuFlightSheetTargetInputModel, GpuFlightSheetTarget>();

        CreateMap<FlightSheetTargetConfig, FlightSheetTargetConfigModel>();
        CreateMap<FlightSheetTargetConfigInputModel, FlightSheetTargetConfig>();

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