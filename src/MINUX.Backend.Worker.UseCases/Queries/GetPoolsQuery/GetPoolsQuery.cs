using MediatR;
using MINUX.Backend.Worker.Core;

namespace MINUX.Backend.Worker.UseCases.Queries.GetPoolsQuery;

public class GetPoolsQuery : IStreamRequest<Pool>
{
}