using AutoMapper;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Commands.Crypto.AddCryptocurrency;
using MNX.MonitoringCenter.Management.UseCases.Commands.Pools;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets;
using MNX.MonitoringCenter.Management.UseCases.Commands.Wallets;

namespace MNX.MonitoringCenter.Management.UseCases;

/// <summary>
/// Конфигурация автомаппера
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AddCryptocurrencyCommand, Cryptocurrency>()
            .ForMember(destination => destination.AlgorithmName, options => options.MapFrom(source => source.Algorithm));

        CreateMap<WalletInputModel, Wallet>()
            .ForMember(destination => destination.Cryptocurrency, options => options.MapFrom(source => source.CryptocurrencyFullName));

        CreateMap<Wallet, WalletModel>()
            .ForMember(destination => destination.Cryptocurrency, options => options.MapFrom(source => source.Cryptocurrency!.FullName));

        CreateMap<PoolInputModel, Pool>()
            .ForMember(destination => destination.Cryptocurrency, options => options.MapFrom(source => source.CryptocurrencyFullName));

        CreateMap<Pool, PoolModel>()
            .ForMember(destination => destination.Cryptocurrency, options => options.MapFrom(source => source.Cryptocurrency!.FullName));

        CreateMap<PresetModel, Preset>();
    }
}