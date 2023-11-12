using MediatR;
using MINUX.Backend.Unit.Core.StaticData;

namespace MINUX.Backend.Unit.UseCases.Queries.GetRamDataQuery;

public class GetRamsDataQueryHandler : IStreamRequestHandler<GetRamsDataQuery, Ram>
{
    public IAsyncEnumerable<Ram> Handle(GetRamsDataQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
