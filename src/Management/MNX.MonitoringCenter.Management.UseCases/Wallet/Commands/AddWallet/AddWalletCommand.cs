using MNX.Application.UseCases;
using MNX.MonitoringCenter.Management.Contracts;

namespace MNX.MonitoringCenter.Management.UseCases.Wallet.Commands.AddWallet;

/// <summary>
/// Команда добавления кошелька
/// </summary>
public class AddWalletCommand : IValidatableCommand<WalletModel>
{
    /// <summary>
    /// Модель кошелька
    /// </summary>
    public WalletInputModel Model { get; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; }

    public AddWalletCommand(WalletInputModel model, Guid userId)
    {
        Model = model;
        UserId = userId;
    }
}
