using AutoMapper;
using MediatR;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.UseCases.Mining.Cryptocurrency;
using System.Runtime.CompilerServices;

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

    public GetCryptocurrenciesQuery(Guid userId)
    {
        Specification = new Specification(userId);
    }

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

    private readonly IMapper _mapper;

    public GetCryptocurrenciesQueryHandler(ICryptocurrencyRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async IAsyncEnumerable<CryptocurrencyModel> Handle(GetCryptocurrenciesQuery request,
                                                             [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var cryptocurrencies = _repository.GetAllAvailable(request.Specification);

        await foreach (var cryptocurrency in cryptocurrencies.WithCancellation(cancellationToken))
        {
            yield return _mapper.Map<CryptocurrencyModel>(cryptocurrency);
        }
    }
}
