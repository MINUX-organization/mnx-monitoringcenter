using MNX.Application.UseCases;
using MNX.MonitoringCenter.Management.Contracts;

namespace MNX.MonitoringCenter.Management.UseCases.Wallet.Commands.EditWallet;

/// <summary>
/// Команда редактирования данных кошелька
/// </summary>
public class EditWalletCommand : IValidatableCommand<WalletModel>
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Модель кошелька
    /// </summary>
    public WalletInputModel Model { get; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public int UserId { get; set; }

    public EditWalletCommand(Guid id, WalletInputModel model, int userId)
    {
        Id = id;
        Model = model;
        UserId = userId;
    }
}
