using Kernel.UseCases;
using MediatR;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Pools.AddPool;

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