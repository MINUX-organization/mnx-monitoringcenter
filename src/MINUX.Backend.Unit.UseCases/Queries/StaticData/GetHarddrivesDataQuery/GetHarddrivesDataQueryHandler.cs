using MediatR;
using MINUX.Backend.Unit.Core.StaticData.Harddrive;

namespace MINUX.Backend.Unit.UseCases.Queries.GetHarddriveDataQuery;

public class GetHarddrivesDataQueryHandler : IStreamRequestHandler<GetHarddrivesDataQuery, Harddrive>
{
    public IAsyncEnumerable<Harddrive> Handle(GetHarddrivesDataQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
