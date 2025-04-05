using MediatR;
using MNX.Application.UseCases.Results;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Pool.Commands.RemovePool;

/// <summary>
/// Обработчик команды удаления пула.
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
        var pool = await _repository.GetAvailableById(request.Id, request.UserId, cancellationToken);

        if (pool is null)
            return Result<Unit>.Empty();

        if (pool.IsDomain())
            return Result<Unit>.Invalid("Domain pool cannot be deleted");

        await _repository.Remove(request.Id, request.UserId);
        return Result<Unit>.Empty();
    }
}
