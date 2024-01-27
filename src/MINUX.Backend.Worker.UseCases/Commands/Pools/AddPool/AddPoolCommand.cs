using Kernel.UseCases;
using MediatR;

namespace MINUX.Backend.Worker.UseCases.Commands.Pools.AddPool;

/// <summary>
/// Команда добавления пула
/// </summary>
public class AddPoolCommand : IRequest<Result<Guid>>
{
    /// <summary>
    /// Модель пула
    /// </summary>
    public PoolModel Model { get; }

    public AddPoolCommand(PoolModel model)
    {
        Model = model;
    }
}