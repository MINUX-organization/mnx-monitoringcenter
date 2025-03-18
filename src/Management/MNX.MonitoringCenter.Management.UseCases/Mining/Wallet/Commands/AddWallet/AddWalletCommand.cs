using MNX.Application.UseCases.Requests;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.UseCases.Mining.Wallet.Commands;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Wallet.Commands.AddWallet;

/// <summary>
/// Команда добавления кошелька
/// </summary>
public class AddWalletCommand : IUserableValidatableCommand<WalletModel>
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
