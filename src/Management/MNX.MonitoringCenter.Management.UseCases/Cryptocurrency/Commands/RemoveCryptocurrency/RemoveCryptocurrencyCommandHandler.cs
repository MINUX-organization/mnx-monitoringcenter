using MediatR;
using MNX.Application.UseCases;

namespace MNX.MonitoringCenter.Management.UseCases.Cryptocurrency.Commands.RemoveCryptocurrency;

/// <summary>
/// Обработчик команды удаления криптовалюты
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
        var cryptocurrency = await _repository.GetAvailableById(request.Id, request.UserId);

        if (cryptocurrency is not null)
        {
            await _repository.Remove(cryptocurrency);
        }

        return Result<Unit>.Empty();
    }
}
