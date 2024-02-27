using MediatR;
using MNX.MonitoringCenter.Management.Contracts;

namespace MNX.MonitoringCenter.Management.UseCases.Queries.GetPoolsQuery;

public class GetPoolsQuery : IStreamRequest<PoolModel>
{
}