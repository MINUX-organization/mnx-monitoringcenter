using AutoMapper;
using MediatR;
using MNX.MonitoringCenter.Management.Contracts;
using System.Runtime.CompilerServices;

namespace MNX.MonitoringCenter.Management.UseCases.Cryptocurrency.Queries;

/// <summary>
/// Запрос на получение списка криптовалют.
/// </summary>
/// <param name="UserId"> Идентификатор пользователя. </param>
public sealed record GetCryptocurrenciesQuery(Guid UserId) : IStreamRequest<CryptocurrencyModel>;

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
        await foreach (var cryptocurrency in _repository.GetAllAvailable(request.UserId))
        {
            yield return _mapper.Map<CryptocurrencyModel>(cryptocurrency);
        }
    }
}
