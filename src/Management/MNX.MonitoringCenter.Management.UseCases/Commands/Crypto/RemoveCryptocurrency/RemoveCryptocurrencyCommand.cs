using Kernel.UseCases;
using MediatR;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Crypto.RemoveCryptocurrency;

/// <summary>
/// Команда удаления криптовалюты
/// </summary>
public class RemoveCryptocurrencyCommand : IRequest<Result<Unit>>
{
    /// <summary>
    /// Идентификатор криптовалюты
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public long UserId { get; set; }

    public RemoveCryptocurrencyCommand(Guid id, long userId)
    {
        Id = id;
        UserId = userId;
    }
}
