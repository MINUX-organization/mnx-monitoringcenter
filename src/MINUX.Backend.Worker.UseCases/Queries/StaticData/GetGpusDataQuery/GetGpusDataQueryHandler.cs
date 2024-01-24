using MediatR;
using MINUX.Backend.Worker.Core.HardwareParameters.Gpu;

namespace MINUX.Backend.Worker.UseCases.Queries.GetGpuDataQuery;

public class GetGpusDataQueryHandler : IStreamRequestHandler<GetGpusDataQuery, Gpu>
{
    public IAsyncEnumerable<Gpu> Handle(GetGpusDataQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
