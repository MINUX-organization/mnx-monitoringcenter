using MediatR;
using MNX.Application.UseCases;

namespace MNX.MonitoringCenter.Management.UseCases.Pool.Commands.RemovePool;

/// <summary>
/// Команда удаления пула
/// </summary>
public class RemovePoolCommand : IRequest<Result<Unit>>
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public long UserId { get; }

    public RemovePoolCommand(Guid id, long userId)
    {
        Id = id;
        UserId = userId;
    }
}
