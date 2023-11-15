using MediatR;
using MINUX.Backend.Unit.Core;

namespace MINUX.Backend.Unit.UseCases.Queries.GetMinersQuery;

public sealed record GetAvailableMinersQuery() : IStreamRequest<Miner>;