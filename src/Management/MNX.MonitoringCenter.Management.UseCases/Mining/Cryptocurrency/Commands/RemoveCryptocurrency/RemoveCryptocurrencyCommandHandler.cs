using MediatR;
using MNX.Application.UseCases.Results;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Cryptocurrency.Commands.RemoveCryptocurrency;

/// <summary>
/// Обработчик команды <see cref="RemoveCryptocurrencyCommand"/>.
/// </summary>
public class RemoveCryptocurrencyCommandHandler : IRequestHandler<RemoveCryptocurrencyCommand, Result<Unit>>
{
    private readonly ICryptocurrencyRepository _repository;

    public RemoveCryptocurrencyCommandHandler(ICryptocurrencyRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Result<Unit>> Handle(RemoveCryptocurrencyCommand request, CancellationToken cancellationToken)
    {
        var cryptocurrency = await _repository.GetAvailableById(request.Id, request.UserId, cancellationToken);

        if (cryptocurrency is null)
            return Result<Unit>.Empty();

        if (cryptocurrency.IsDomain())
            return Result<Unit>.Invalid("Domain cryptocurrency cannot be deleted");

        await _repository.Remove(request.Id, request.UserId);
        return Result<Unit>.Empty();
    }
}
