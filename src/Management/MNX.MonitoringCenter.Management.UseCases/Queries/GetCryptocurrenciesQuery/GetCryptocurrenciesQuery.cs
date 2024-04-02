using MediatR;
using MNX.MonitoringCenter.Management.Core;

namespace MNX.MonitoringCenter.Management.UseCases.Queries.GetCryptocurrenciesQuery;

public class GetCryptocurrenciesQuery : IStreamRequest<Cryptocurrency>
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
