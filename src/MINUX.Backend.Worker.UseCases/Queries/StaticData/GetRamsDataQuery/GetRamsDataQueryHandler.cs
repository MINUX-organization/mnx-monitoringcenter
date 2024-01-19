using MediatR;
using MINUX.Backend.Worker.Core.StaticData;

namespace MINUX.Backend.Worker.UseCases.Queries.GetRamDataQuery;

public class GetRamsDataQueryHandler : IStreamRequestHandler<GetRamsDataQuery, Ram>
{
    public IAsyncEnumerable<Ram> Handle(GetRamsDataQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
