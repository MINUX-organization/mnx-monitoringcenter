using AutoMapper;
using MediatR;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;
using System.Runtime.CompilerServices;

namespace MNX.MonitoringCenter.Management.UseCases.Queries.GetCryptocurrenciesQuery;

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
        await foreach(var cryptocurrency in _repository.GetAll())
        {
            yield return _mapper.Map<CryptocurrencyModel>(cryptocurrency);
        }
    }
}
