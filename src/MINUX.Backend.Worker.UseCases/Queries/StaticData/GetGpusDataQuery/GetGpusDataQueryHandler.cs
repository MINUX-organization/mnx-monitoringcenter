using MediatR;
using MINUX.Backend.Worker.Core.StaticData.Gpu;

namespace MINUX.Backend.Worker.UseCases.Queries.GetGpuDataQuery;

public class GetGpusDataQueryHandler : IStreamRequestHandler<GetGpusDataQuery, Gpu>
{
    public IAsyncEnumerable<Gpu> Handle(GetGpusDataQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
