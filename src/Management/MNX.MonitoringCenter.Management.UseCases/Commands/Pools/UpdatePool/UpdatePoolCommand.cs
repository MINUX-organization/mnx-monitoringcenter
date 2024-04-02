using Kernel.UseCases;
using MediatR;
using MNX.MonitoringCenter.Management.Contracts;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Pools.UpdatePool;

/// <summary>
/// Команда обновления пула
/// </summary>
public class UpdatePoolCommand : IValidateableCommand<PoolModel>
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Модель пула
    /// </summary>
    public PoolInputModel Model { get; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public long UserId { get; }

    public UpdatePoolCommand(Guid id, PoolInputModel model, long userId)
    {
        Id = id;
        Model = model;
        UserId = userId;
    }
}
