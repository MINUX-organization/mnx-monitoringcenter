using MediatR;
using MINUX.Backend.Unit.Core.StaticData.Gpu;

namespace MINUX.Backend.Unit.UseCases.Queries.GetGpuDataQuery;

public class GetGpusDataQueryHandler : IStreamRequestHandler<GetGpusDataQuery, Gpu>
{
    public IAsyncEnumerable<Gpu> Handle(GetGpusDataQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
