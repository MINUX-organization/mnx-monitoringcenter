using AutoMapper;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.UseCases.Mining.Wallet.Commands.AddWallet;
using MNX.MonitoringCenter.Management.UseCases.Mining.Wallet.Commands.EditWallet;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Wallet;

using Wallet = Core.Mining.Wallet;

/// <summary>
/// Профиль маппинга для сущностей, связанных с сущностью <see cref="Wallet"/>.
/// </summary>
public class WalletMappingProfile : Profile
{
    public WalletMappingProfile()
    {
        CreateMap<Wallet, WalletModel>()
            .ForMember(destination => destination.CryptocurrencyId, options => options.MapFrom(source => source.Cryptocurrency!.Id))
            .ForMember(destination => destination.Cryptocurrency, options => options.MapFrom(source => source.Cryptocurrency!.FullName));

        CreateMap<AddWalletCommand, Wallet>()
            .ForMember(destination => destination.Name, options => options.MapFrom(source => source.Model.Name))
            .ForMember(destination => destination.Address, options => options.MapFrom(source => source.Model.Address))
            .ForMember(destination => destination.CryptocurrencyId, options => options.MapFrom(source => source.Model.CryptocurrencyId));

        CreateMap<EditWalletCommand, Wallet>()
            .ForMember(destination => destination.Name, options => options.MapFrom(source => source.Model.Name))
            .ForMember(destination => destination.Address, options => options.MapFrom(source => source.Model.Address))
            .ForMember(destination => destination.CryptocurrencyId, options => options.MapFrom(source => source.Model.CryptocurrencyId));
    }
}
