using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.UseCases.Mining.Wallet;
using MNX.MonitoringCenter.Management.UseCases.Mining.Wallet.Commands.AddWallet;
using MNX.MonitoringCenter.Management.UseCases.Mining.Wallet.Commands.EditWallet;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Wallet;

using Wallet = Core.Mining.Wallet;

/// <summary>
/// Реализация <see cref="IWalletMapper"/>.
/// </summary>
public class WalletMapper : IWalletMapper
{
    /// <inheritdoc/>
    public Wallet MapToCoreEntity(AddWalletCommand model)
    {
        return new Wallet()
        {
            Name = model.Model.Name,
            Address = model.Model.Address,
            CryptocurrencyId = model.Model.CryptocurrencyId,
            OwnerId = model.UserId
        };
    }

    /// <inheritdoc/>
    public Wallet MapToCoreEntity(EditWalletCommand model)
    {
        return new Wallet()
        {
            Id = model.Id,
            Name = model.Model.Name,
            Address = model.Model.Address,
            CryptocurrencyId = model.Model.CryptocurrencyId,
            OwnerId = model.UserId
        };
    }

    /// <inheritdoc/>
    public WalletModel MapToModel(Wallet entity)
    {
        return new WalletModel()
        {
            Id = entity.Id,
            Name = entity.Name,
            Address = entity.Address,
            CryptocurrencyId = entity.Cryptocurrency!.Id,
            Cryptocurrency = entity.Cryptocurrency!.FullName
        };
    }
}
