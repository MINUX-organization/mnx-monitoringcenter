using MediatR;
using MNX.MonitoringCenter.Management.Contracts;

namespace MNX.MonitoringCenter.Management.UseCases.Combinations.Queries;

public class GetAvailableMiningCombinationsQuery : IRequest<Dictionary<(Guid RigId, Guid FlightSheetId, Guid MinerId, Guid CoinId), MiningCombinations>>
{
    public Guid UserId { get; }

    public GetAvailableMiningCombinationsQuery(Guid userId)
    {
        UserId = userId;
    }
}

public class GetAvailableMiningCombinationsQueryHandler : IRequestHandler<GetAvailableMiningCombinationsQuery, Dictionary<(Guid RigId, Guid FlightSheetId, Guid MinerId, Guid CoinId), MiningCombinations>>
{
    public Task<Dictionary<(Guid RigId, Guid FlightSheetId, Guid MinerId, Guid CoinId), MiningCombinations>> Handle(GetAvailableMiningCombinationsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}