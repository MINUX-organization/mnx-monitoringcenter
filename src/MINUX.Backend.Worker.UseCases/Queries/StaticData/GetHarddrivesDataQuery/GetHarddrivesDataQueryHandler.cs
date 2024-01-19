using MediatR;
using MINUX.Backend.Worker.Core.StaticData.Harddrive;

namespace MINUX.Backend.Worker.UseCases.Queries.GetHarddriveDataQuery;

public class GetHarddrivesDataQueryHandler : IStreamRequestHandler<GetHarddrivesDataQuery, Harddrive>
{
    public IAsyncEnumerable<Harddrive> Handle(GetHarddrivesDataQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
