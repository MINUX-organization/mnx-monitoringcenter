using MediatR;
using MINUX.Backend.Worker.Core.HardwareParameters.Harddrive;

namespace MINUX.Backend.Worker.UseCases.Queries.GetHarddriveDataQuery;

public class GetHarddrivesDataQuery : IStreamRequest<Harddrive>
{
}
