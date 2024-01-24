using MediatR;
using MINUX.Backend.Worker.Core.HardwareParameters;

namespace MINUX.Backend.Worker.UseCases.Queries.GetRamDataQuery;

public class GetRamsDataQuery : IStreamRequest<Ram>
{
}
