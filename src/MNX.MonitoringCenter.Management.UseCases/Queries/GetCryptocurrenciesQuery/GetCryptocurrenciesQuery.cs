using MediatR;
using MNX.MonitoringCenter.Management.Core;

namespace MNX.MonitoringCenter.Management.UseCases.Queries.GetCryptocurrenciesQuery;

public class GetCryptocurrenciesQuery : IStreamRequest<Cryptocurrency>
{
}
