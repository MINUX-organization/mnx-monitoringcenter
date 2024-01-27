using Kernel.UseCases;
using MediatR;

namespace MINUX.Backend.Worker.UseCases.Commands.Wallets.AddWallet;

/// <summary>
/// Команда добавления кошелька
/// </summary>
public class AddWalletCommand : IRequest<Result<Guid>>
{
    /// <summary>
    /// Модель кошелька
    /// </summary>
    public WalletModel Model { get; }

    public AddWalletCommand(WalletModel model)
    {
        Model = model;
    }
}
