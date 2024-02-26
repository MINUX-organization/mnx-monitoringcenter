using MediatR;

namespace MNX.MonitoringCenter.Management.UseCases.Queries.GetMinersQuery;

public sealed record GetAvailableMinersQuery() : IStreamRequest<string>;