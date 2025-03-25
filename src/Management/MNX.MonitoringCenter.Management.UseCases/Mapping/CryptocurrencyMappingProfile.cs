using AutoMapper;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.Core.Mining;
using MNX.MonitoringCenter.Management.UseCases.Mining.Cryptocurrency.Commands.AddCryptocurrency;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping;

/// <summary>
/// Профиль маппинга для сущностей, связанных с сущностью <see cref="Cryptocurrency"/>.
/// </summary>
public class CryptocurrencyMappingProfile : Profile
{
    public CryptocurrencyMappingProfile()
    {
        CreateMap<AddCryptocurrencyCommand, Cryptocurrency>()
            .ForMember(destination => destination.ShortName, options => options.MapFrom(source => source.Model.ShortName))
            .ForMember(destination => destination.FullName, options => options.MapFrom(source => source.Model.FullName))
            .ForMember(destination => destination.AlgorithmId, options => options.MapFrom(source => source.Model.AlgorithmId));

        CreateMap<Cryptocurrency, CryptocurrencyModel>();
    }
}
