using Kernel.UseCases;
using MediatR;
using MNX.MonitoringCenter.Management.Contracts;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Wallets.EditWallet;

/// <summary>
/// Команда редактирования данных кошелька
/// </summary>
public class EditWalletCommand : IValidateableCommand<WalletModel>
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
    public long UserId { get; set; }

    public EditWalletCommand(Guid id, WalletInputModel model, long userId)
    {
        Id = id;
        Model = model;
        UserId = userId;
    }
}
