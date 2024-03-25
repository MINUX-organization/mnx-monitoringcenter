using MediatR;
using MNX.MonitoringCenter.Management.Core;

namespace MNX.MonitoringCenter.Management.UseCases.Queries.GetCryptocurrenciesQuery;

public class GetFlightSheetsQuery : IStreamRequest<FlightSheet>
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public long UserId { get; set; }

    public GetFlightSheetsQuery(long userId)
    {
        UserId = userId;
    }
}
