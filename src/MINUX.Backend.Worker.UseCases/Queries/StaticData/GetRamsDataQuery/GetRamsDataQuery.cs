using MediatR;
using MINUX.Backend.Worker.Core.StaticData;

namespace MINUX.Backend.Worker.UseCases.Queries.GetRamDataQuery;

public class GetRamsDataQuery : IStreamRequest<Ram>
{
}
