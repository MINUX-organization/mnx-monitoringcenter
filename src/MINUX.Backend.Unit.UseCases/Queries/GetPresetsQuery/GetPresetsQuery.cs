using MediatR;
using MINUX.Backend.Unit.Core;

namespace MINUX.Backend.Unit.UseCases.Queries.GetPresetsQuery;

public class GetPresetsQuery : IStreamRequest<Preset>
{
}