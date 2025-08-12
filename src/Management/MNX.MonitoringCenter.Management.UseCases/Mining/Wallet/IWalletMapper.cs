using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.UseCases.Mining.Wallet.Commands.AddWallet;
using MNX.MonitoringCenter.Management.UseCases.Mining.Wallet.Commands.EditWallet;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Wallet;

using Wallet = Core.Mining.Wallet;

/// <summary>
/// Интерфейс маппера сущности <see cref="Wallet"/> и её моделей.
/// </summary>
public interface IWalletMapper
{
    /// <summary>
    /// Преобразовать <see cref="AddWalletCommand"/> в <see cref="Wallet"/>.
    /// </summary>
    /// <param name="model"> Модель данных. </param>
    /// <returns> Новый экземпляр <see cref="Wallet"/>. </returns>
    Wallet MapToCoreEntity(AddWalletCommand model);

    /// <summary>
    /// Преобразовать <see cref="EditWalletCommand"/> в <see cref="Wallet"/>.
    /// </summary>
    /// <param name="model"> Модель данных. </param>
    /// <returns> Новый экземпляр <see cref="Wallet"/>. </returns>
    Wallet MapToCoreEntity(EditWalletCommand model);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="entity"> Модель данных. </param>
    /// <returns> Новый экземпляр <see cref="WalletModel"/>. </returns>
    WalletModel MapToModel(Wallet entity);
}
