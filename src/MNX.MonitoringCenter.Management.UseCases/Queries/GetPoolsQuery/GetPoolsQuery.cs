using MediatR;
using MNX.MonitoringCenter.Management.Core;

namespace MNX.MonitoringCenter.Management.UseCases.Queries.GetPoolsQuery;

public class GetPoolsQuery : IStreamRequest<Pool>
{
}