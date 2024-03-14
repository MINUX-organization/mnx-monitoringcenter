using Kernel.UseCases;
using MediatR;
using MNX.MonitoringCenter.Management.Contracts;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Pools.AddPool;

/// <summary>
/// Команда добавления пула
/// </summary>
public class AddPoolCommand : IRequest<Result<PoolModel>>
{
    /// <summary>
    /// Модель пула
    /// </summary>
    public PoolInputModel Model { get; }

    public AddPoolCommand(PoolInputModel model)
    {
        Model = model;
    }
}