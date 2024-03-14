using Kernel.UseCases;
using MediatR;
using MNX.MonitoringCenter.Management.Contracts;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Pools.UpdatePool;

/// <summary>
/// Команда обновления пула
/// </summary>
public class UpdatePoolCommand : IRequest<Result<PoolModel>>
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Модель пула
    /// </summary>
    public PoolInputModel Model { get; }

    public UpdatePoolCommand(Guid id, PoolInputModel model)
    {
        Id = id;
        Model = model;
    }
}
