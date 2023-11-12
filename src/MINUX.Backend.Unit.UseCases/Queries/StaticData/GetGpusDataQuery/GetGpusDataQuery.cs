using MediatR;
using MINUX.Backend.Unit.Core.StaticData.Gpu;

namespace MINUX.Backend.Unit.UseCases.Queries.GetGpuDataQuery;

public class GetGpusDataQuery : IStreamRequest<Gpu>
{
}
