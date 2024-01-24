using MediatR;
using MINUX.Backend.Worker.Core.HardwareParameters.Gpu;

namespace MINUX.Backend.Worker.UseCases.Queries.GetGpuDataQuery;

public class GetGpusDataQuery : IStreamRequest<Gpu>
{
}
