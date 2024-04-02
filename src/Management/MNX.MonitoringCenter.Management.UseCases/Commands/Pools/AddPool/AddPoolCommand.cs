using MNX.Application.UseCases;
using MNX.MonitoringCenter.Management.Contracts;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Pools.AddPool;

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
    public long UserId { get; }

    public AddPoolCommand(PoolInputModel model, long userId)
    {
        Model = model;
        UserId = userId;
    }
}