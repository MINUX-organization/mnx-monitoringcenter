using AutoMapper;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.UseCases.Mining.Pool.Commands.AddPool;
using MNX.MonitoringCenter.Management.UseCases.Mining.Pool.Commands.EditPool;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Pool;

using Pool = Core.Mining.Pool;

/// <summary>
/// Профиль маппинга для сущностей, связанных с сущностью <see cref="Pool"/>.
/// </summary>
public class PoolMappingProfile : Profile
{
    public PoolMappingProfile()
    {
        CreateMap<Pool, PoolModel>()
            .ForMember(destination => destination.CryptocurrencyId, options => options.MapFrom(source => source.Cryptocurrency!.Id))
            .ForMember(destination => destination.Cryptocurrency, options => options.MapFrom(source => source.Cryptocurrency!.FullName));

        CreateMap<AddPoolCommand, Pool>()
            .ForMember(destination => destination.Port, options => options.MapFrom(source => source.Model.Port))
            .ForMember(destination => destination.Domain, options => options.MapFrom(source => source.Model.Domain))
            .ForMember(destination => destination.CryptocurrencyId, options => options.MapFrom(source => source.Model.CryptocurrencyId))
            .ForMember(destination => destination.Tls, options => options.MapFrom(source => source.Model.Tls))
            .ForMember(destination => destination.OwnerId, options => options.MapFrom(source => source.UserId));

        CreateMap<EditPoolCommand, Pool>()
            .ForMember(destination => destination.Domain, options => options.MapFrom(source => source.Model.Domain))
            .ForMember(destination => destination.Port, options => options.MapFrom(source => source.Model.Port))
            .ForMember(destination => destination.CryptocurrencyId, options => options.MapFrom(source => source.Model.CryptocurrencyId))
            .ForMember(destination => destination.Tls, options => options.MapFrom(source => source.Model.Tls));
    }
}
