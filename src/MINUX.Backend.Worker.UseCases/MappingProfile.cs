using AutoMapper;
using MINUX.Backend.Worker.Core;
using MINUX.Backend.Worker.UseCases.Commands.AddCryptocurrencyCommand;
using MINUX.Backend.Worker.UseCases.Commands.AddPoolCommand;
using MINUX.Backend.Worker.UseCases.Commands.SavePresetCommand;
using MINUX.Backend.Worker.UseCases.Commands.AddWalletCommand;

namespace MINUX.Backend.Worker.UseCases;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AddCryptocurrencyCommand, Cryptocurrency>()
            .ForMember(destination => destination.AlgorithmName, options => options.MapFrom(source => source.Algorithm));

        CreateMap<AddWalletCommand, Wallet>();

        CreateMap<AddPoolCommand, Pool>();

        CreateMap<SavePresetCommand, Preset>();
    }
}