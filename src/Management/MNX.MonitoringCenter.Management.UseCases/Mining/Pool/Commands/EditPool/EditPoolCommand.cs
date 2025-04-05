using MNX.Application.UseCases.Requests;
using MNX.MonitoringCenter.Management.Contracts;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Pool.Commands.EditPool;

/// <summary>
/// Команда обновления пула.
/// </summary>
public class EditPoolCommand : IUserableValidatableCommand<PoolModel>
{
    /// <summary>
    /// Уникальный идентификатор.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Модель пула.
    /// </summary>
    public PoolInputModel Model { get; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; }

    public EditPoolCommand(Guid id, PoolInputModel model, Guid userId)
    {
        Id = id;
        Model = model;
        UserId = userId;
    }
}
