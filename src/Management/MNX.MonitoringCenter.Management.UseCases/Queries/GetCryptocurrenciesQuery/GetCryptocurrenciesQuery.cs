using MediatR;
using MNX.MonitoringCenter.Management.Contracts;

namespace MNX.MonitoringCenter.Management.UseCases.Queries.GetCryptocurrenciesQuery;

public class GetCryptocurrenciesQuery : IStreamRequest<CryptocurrencyModel>
{
}
