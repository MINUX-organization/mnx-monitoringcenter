using AutoMapper;
using MINUX.Backend.Worker.Core;
using MINUX.Backend.Worker.UseCases.Commands.AddCryptocurrencyCommand;
using MINUX.Backend.Worker.UseCases.Commands.AddPoolCommand;
using MINUX.Backend.Worker.UseCases.Commands.Presets;
using MINUX.Backend.Worker.UseCases.Commands.Wallets;
using MINUX.Backend.Worker.UseCases.Commands.Wallets.AddWallet;

namespace MINUX.Backend.Worker.UseCases;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AddCryptocurrencyCommand, Cryptocurrency>()
            .ForMember(destination => destination.AlgorithmName, options => options.MapFrom(source => source.Algorithm));

        CreateMap<WalletModel, Wallet>()
            .ForMember(destination => destination.Cryptocurrency, options => options.MapFrom(source => source.CryptocurrencyFullName));

        CreateMap<AddPoolCommand, Pool>();

        CreateMap<PresetModel, Preset>();
    }
}