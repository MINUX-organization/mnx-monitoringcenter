using Kernel.UseCases;
using MediatR;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Pools.UpdatePool;

/// <summary>
/// Команда обновления пула
/// </summary>
public class UpdatePoolCommand : IRequest<Result<Unit>>
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Модель пула
    /// </summary>
    public PoolModel Model { get; }

    public UpdatePoolCommand(Guid id, PoolModel model)
    {
        Id = id;
        Model = model;
    }
}
