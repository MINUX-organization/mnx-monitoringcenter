using MediatR;
using MNX.MonitoringCenter.Management.Contracts;

namespace MNX.MonitoringCenter.Management.UseCases.Queries.GetPoolsQuery;

public class GetPoolsQuery : IStreamRequest<PoolModel>
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public long UserId { get; set; }

    public GetPoolsQuery(long userId)
    {
        UserId = userId;
    }
}