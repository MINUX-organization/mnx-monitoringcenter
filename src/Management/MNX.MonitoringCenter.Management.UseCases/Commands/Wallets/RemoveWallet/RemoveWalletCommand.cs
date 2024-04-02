using Kernel.UseCases;
using MediatR;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Wallets.RemoveWallet;

/// <summary>
/// Команда удаления кошелька
/// </summary>
public class RemoveWalletCommand : IRequest<Result<Unit>>
{
    /// <summary>
    /// Уникальный идентификатор кошелька
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public long UserId { get; set; }

    public RemoveWalletCommand(Guid id, long userId)
    {
        Id = id;
        UserId = userId;
    }
}
