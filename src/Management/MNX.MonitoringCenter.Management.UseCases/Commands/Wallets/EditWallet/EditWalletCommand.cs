using Kernel.UseCases;
using MediatR;
using MNX.MonitoringCenter.Management.Contracts;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Wallets.EditWallet;

/// <summary>
/// Команда редактирования данных кошелька
/// </summary>
public class EditWalletCommand : IRequest<Result<WalletModel>>
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Модель кошелька
    /// </summary>
    public WalletInputModel Model { get; }

    public EditWalletCommand(Guid id, WalletInputModel model)
    {
        Id = id;
        Model = model;
    }
}
