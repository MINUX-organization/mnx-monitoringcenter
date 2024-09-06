using MNX.Application.UseCases;
using MNX.MonitoringCenter.Management.Contracts;

namespace MNX.MonitoringCenter.Management.UseCases.Pool.Commands.AddPool;

/// <summary>
/// Команда добавления пула
/// </summary>
public class AddPoolCommand : IValidatableCommand<PoolModel>
{
    /// <summary>
    /// Модель пула
    /// </summary>
    public PoolInputModel Model { get; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; }

    public AddPoolCommand(PoolInputModel model, Guid userId)
    {
        Model = model;
        UserId = userId;
    }
}