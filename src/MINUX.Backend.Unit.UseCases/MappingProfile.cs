using AutoMapper;
using MINUX.Backend.Unit.Core;
using MINUX.Backend.Unit.UseCases.Commands.AddCryptocurrencyCommand;
using MINUX.Backend.Unit.UseCases.Commands.AddPoolCommand;
using MINUX.Backend.Unit.UseCases.Commands.AddWalletCommand;

namespace MINUX.Backend.Unit.UseCases;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AddCryptocurrencyCommand, Cryptocurrency>()
            .ForMember(destination => destination.AlgorithmName, options => options.MapFrom(source => source.Algorithm));

        CreateMap<AddWalletCommand, Wallet>();

        CreateMap<AddPoolCommand, Pool>();
    }
}