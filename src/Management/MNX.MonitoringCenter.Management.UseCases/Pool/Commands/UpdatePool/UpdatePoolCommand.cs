using MNX.Application.UseCases;
using MNX.MonitoringCenter.Management.Contracts;

namespace MNX.MonitoringCenter.Management.UseCases.Pool.Commands.UpdatePool;

/// <summary>
/// Команда обновления пула
/// </summary>
public class UpdatePoolCommand : IValidatableCommand<PoolModel>
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
    public Guid UserId { get; }

    public UpdatePoolCommand(Guid id, PoolInputModel model, Guid userId)
    {
        Id = id;
        Model = model;
        UserId = userId;
    }
}
