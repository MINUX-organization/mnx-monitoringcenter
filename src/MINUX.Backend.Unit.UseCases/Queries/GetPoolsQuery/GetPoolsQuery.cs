using MediatR;
using MINUX.Backend.Unit.Core;

namespace MINUX.Backend.Unit.UseCases.Queries.GetPoolsQuery;

public class GetPoolsQuery : IStreamRequest<Pool>
{
}