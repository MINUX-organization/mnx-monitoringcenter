using Kernel.UseCases;
using MediatR;

namespace MINUX.Backend.Worker.UseCases.Commands.Pools.RemovePool;

/// <summary>
/// Команда удаления пула
/// </summary>
public class RemovePoolCommand : IRequest<Result<Unit>>
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public Guid Id { get; }

    public RemovePoolCommand(Guid id)
    {
        Id = id;
    }
}
