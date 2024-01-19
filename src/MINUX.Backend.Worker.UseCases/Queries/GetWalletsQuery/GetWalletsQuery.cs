using MediatR;
using MINUX.Backend.Worker.Core;

namespace MINUX.Backend.Worker.UseCases.Queries.GetWalletsQuery;

public class GetWalletsQuery : IStreamRequest<Wallet>
{
}