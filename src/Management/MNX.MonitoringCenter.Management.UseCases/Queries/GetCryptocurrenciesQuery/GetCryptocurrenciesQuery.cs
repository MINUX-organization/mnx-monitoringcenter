using MediatR;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.Core;

namespace MNX.MonitoringCenter.Management.UseCases.Queries.GetCryptocurrenciesQuery;

public class GetCryptocurrenciesQuery : IStreamRequest<CryptocurrencyModel>
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public long UserId { get; set; }

    public GetCryptocurrenciesQuery(long userId)
    {
        UserId = userId;
    }
}
