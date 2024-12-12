using MNX.Application.UseCases.Requests;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.UseCases.Mining.Pool.Commands;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Pool.Commands.AddPool;

/// <summary>
/// Команда добавления пула
/// </summary>
public class AddPoolCommand : IUserableValidatableCommand<PoolModel>
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