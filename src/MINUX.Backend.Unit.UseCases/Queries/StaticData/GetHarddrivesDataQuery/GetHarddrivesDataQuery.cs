using MediatR;
using MINUX.Backend.Unit.Core.StaticData.Harddrive;

namespace MINUX.Backend.Unit.UseCases.Queries.GetHarddriveDataQuery;

public class GetHarddrivesDataQuery : IStreamRequest<Harddrive>
{
}
