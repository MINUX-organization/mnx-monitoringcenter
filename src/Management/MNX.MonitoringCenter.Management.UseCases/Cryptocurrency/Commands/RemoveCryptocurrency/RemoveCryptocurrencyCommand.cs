using MediatR;
using MNX.Application.UseCases.Results;

namespace MNX.MonitoringCenter.Management.UseCases.Cryptocurrency.Commands.RemoveCryptocurrency;

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
    public Guid UserId { get; }

    public RemoveCryptocurrencyCommand(Guid id, Guid userId)
    {
        Id = id;
        UserId = userId;
    }
}
