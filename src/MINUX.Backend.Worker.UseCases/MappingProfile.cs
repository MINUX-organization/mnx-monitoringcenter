using AutoMapper;
using MINUX.Backend.Worker.Core;
using MINUX.Backend.Worker.UseCases.Commands.AddCryptocurrencyCommand;
using MINUX.Backend.Worker.UseCases.Commands.Pools;
using MINUX.Backend.Worker.UseCases.Commands.Presets;
using MINUX.Backend.Worker.UseCases.Commands.Wallets;

namespace MINUX.Backend.Worker.UseCases;

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