using Kernel.UseCases;
using MediatR;

namespace MINUX.Backend.Worker.UseCases.Commands.Wallets.RemoveWallet;

/// <summary>
/// Команда удаления кошелька
/// </summary>
public class RemoveWalletCommand : IRequest<Result<Unit>>
{
    /// <summary>
    /// Уникальный идентификатор кошелька
    /// </summary>
    public Guid Id { get; }

    public RemoveWalletCommand(Guid id)
    {
        Id = id;
    }
}
