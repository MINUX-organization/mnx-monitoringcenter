using MediatR;
using MNX.Application.UseCases.Results;

namespace MNX.MonitoringCenter.Management.UseCases.Pool.Commands.RemovePool;

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
        await _repository.Remove(request.Id, request.UserId);
        return Result<Unit>.Empty();
    }
}
