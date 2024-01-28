using Kernel.UseCases;
using MediatR;
using MINUX.Backend.Worker.UseCases.Abstractions;

namespace MINUX.Backend.Worker.UseCases.Commands.Crypto.RemoveCryptocurrency;

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
        var cryptocurrency = await _repository.GetByFullName(request.FullName);

        if (cryptocurrency is null)
        {
            return Result<Unit>.Invalid("Cryptocurrency wasn't found");
        }

        await _repository.Remove(cryptocurrency);
        return Result<Unit>.Empty();
    }
}
