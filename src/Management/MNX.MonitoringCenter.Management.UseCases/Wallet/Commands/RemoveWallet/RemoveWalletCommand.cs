using MediatR;
using MNX.Application.UseCases;

namespace MNX.MonitoringCenter.Management.UseCases.Wallet.Commands.RemoveWallet;

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
