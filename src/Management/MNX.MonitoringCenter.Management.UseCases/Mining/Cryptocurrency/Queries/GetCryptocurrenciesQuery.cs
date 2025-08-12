using MediatR;
using System.Runtime.CompilerServices;
using MNX.MonitoringCenter.Management.Contracts;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Cryptocurrency.Queries;

/// <summary>
/// Запрос на получение списка криптовалют.
/// </summary>
public sealed record GetCryptocurrenciesQuery : IStreamRequest<CryptocurrencyModel>
{
    /// <summary>
    /// Спецификация.
    /// </summary>
    public Specification Specification { get; }

    ///
    public GetCryptocurrenciesQuery(Guid userId)
    {
        Specification = new Specification(userId);
    }

    ///
    public GetCryptocurrenciesQuery(Guid userId, string filterString, object[] filterParameters)
    {
        Specification = new Specification(userId, filterString, filterParameters);
    }
}

/// <summary>
/// Обработчик <see cref="GetCryptocurrenciesQuery"/>.
/// </summary>
public class GetCryptocurrenciesQueryHandler : IStreamRequestHandler<GetCryptocurrenciesQuery, CryptocurrencyModel>
{
    private readonly ICryptocurrencyRepository _repository;

    private readonly ICryptocurrencyMapper _cryptocurrencyMapper;

    ///
    public GetCryptocurrenciesQueryHandler(ICryptocurrencyRepository repository, ICryptocurrencyMapper cryptocurrencyMapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _cryptocurrencyMapper = cryptocurrencyMapper ?? throw new ArgumentNullException(nameof(cryptocurrencyMapper));
    }

    ///
    public async IAsyncEnumerable<CryptocurrencyModel> Handle(GetCryptocurrenciesQuery request,
                                                             [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var cryptocurrencies = _repository.GetAllAvailable(request.Specification);

        await foreach (var cryptocurrency in cryptocurrencies.WithCancellation(cancellationToken))
        {
            yield return _cryptocurrencyMapper.MapToModel(cryptocurrency);
        }
    }
}
