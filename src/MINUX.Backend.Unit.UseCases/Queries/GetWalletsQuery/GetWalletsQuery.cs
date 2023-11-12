using MediatR;
using MINUX.Backend.Unit.Core;

namespace MINUX.Backend.Unit.UseCases.Queries.GetWalletsQuery;

public class GetWalletsQuery : IStreamRequest<Wallet>
{
}