using Kernel.UseCases;
using MediatR;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Pools.RemovePool;

/// <summary>
/// Обработчик команды удаления пула
/// </summary>
public class RemovePoolCommandHandler : IRequestHandler<RemovePoolCommand, Result<Unit>>
{
    private readonly IPoolRepository _repository;

    public RemovePoolCommandHandler(IPoolRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Result<Unit>> Handle(RemovePoolCommand request, CancellationToken cancellationToken)
    {
        var pool = await _repository.GetAvailableById(request.Id, request.UserId);

        if (pool == null)
        {
            return Result<Unit>.Invalid("Pool with this id wasn`t found");
        }

        await _repository.Remove(pool);
        return Result<Unit>.Empty();
    }
}
