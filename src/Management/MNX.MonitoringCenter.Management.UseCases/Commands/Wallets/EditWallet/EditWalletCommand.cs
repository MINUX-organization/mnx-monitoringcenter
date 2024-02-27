using Kernel.UseCases;
using MediatR;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Wallets.EditWallet;

/// <summary>
/// Команда редактирования данных кошелька
/// </summary>
public class EditWalletCommand : IRequest<Result<Unit>>
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
