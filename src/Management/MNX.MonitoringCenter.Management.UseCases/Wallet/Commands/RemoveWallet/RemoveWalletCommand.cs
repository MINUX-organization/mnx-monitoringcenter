using MediatR;
using MNX.Application.UseCases.Results;

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
    public Guid UserId { get; }

    public RemoveWalletCommand(Guid id, Guid userId)
    {
        Id = id;
        UserId = userId;
    }
}
