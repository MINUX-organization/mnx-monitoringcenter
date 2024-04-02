using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Management.Contracts;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Wallets.AddWallet;

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
    public long UserId { get; set; }

    public AddWalletCommand(WalletInputModel model, long userId)
    {
        Model = model;
        UserId = userId;
    }
}
