using MediatR;
using MINUX.Backend.Worker.Core.HardwareParameters;

namespace MINUX.Backend.Worker.UseCases.Queries.GetRamDataQuery;

public class GetRamsDataQueryHandler : IStreamRequestHandler<GetRamsDataQuery, Ram>
{
    public IAsyncEnumerable<Ram> Handle(GetRamsDataQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
