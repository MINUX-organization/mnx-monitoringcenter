using MediatR;
using MNX.MonitoringCenter.Management.Core;

namespace MNX.MonitoringCenter.Management.UseCases.Queries.GetMinersQuery;

public sealed record GetAvailableMinersQuery() : IStreamRequest<Miner>;