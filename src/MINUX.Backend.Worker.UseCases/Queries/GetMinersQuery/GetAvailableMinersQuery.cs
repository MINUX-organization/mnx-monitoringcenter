using MediatR;
using MINUX.Backend.Worker.Core;

namespace MINUX.Backend.Worker.UseCases.Queries.GetMinersQuery;

public sealed record GetAvailableMinersQuery() : IStreamRequest<Miner>;