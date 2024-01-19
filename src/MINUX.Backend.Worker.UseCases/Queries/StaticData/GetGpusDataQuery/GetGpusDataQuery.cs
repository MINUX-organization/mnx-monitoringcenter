using MediatR;
using MINUX.Backend.Worker.Core.StaticData.Gpu;

namespace MINUX.Backend.Worker.UseCases.Queries.GetGpuDataQuery;

public class GetGpusDataQuery : IStreamRequest<Gpu>
{
}
