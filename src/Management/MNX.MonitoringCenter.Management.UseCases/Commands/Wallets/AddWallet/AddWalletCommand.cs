using Kernel.UseCases;
using MediatR;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Wallets.AddWallet;

/// <summary>
/// Команда добавления кошелька
/// </summary>
public class AddWalletCommand : IRequest<Result<Guid>>
{
    /// <summary>
    /// Модель кошелька
    /// </summary>
    public WalletInputModel Model { get; }

    public AddWalletCommand(WalletInputModel model)
    {
        Model = model;
    }
}
