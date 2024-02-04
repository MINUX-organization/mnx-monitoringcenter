using AutoMapper;
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

        CreateMap<WalletModel, Wallet>()
            .ForMember(destination => destination.Cryptocurrency, options => options.MapFrom(source => source.CryptocurrencyFullName));

        CreateMap<PoolModel, Pool>()
            .ForMember(destination => destination.Cryptocurrency, options => options.MapFrom(source => source.CryptocurrencyFullName));

        CreateMap<PresetModel, Preset>();
    }
}