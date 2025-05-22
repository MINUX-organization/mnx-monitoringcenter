using MediatR;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs;

namespace MNX.MonitoringCenter.Inventory.UseCases.Rigs.Queries;

/// <summary>
/// Обработчик команды <see cref="GetRigsIdsByMinerCoincidenceQuery"/>.
/// </summary>
public class GetRigsIdsByMinerCoincidenceQueryHandler
    : IRequestHandler<GetRigsIdsByMinerCoincidenceQuery, Guid[]>
{
    private readonly IRigRepository _repository;

    public GetRigsIdsByMinerCoincidenceQueryHandler(IRigRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Guid[]> Handle(GetRigsIdsByMinerCoincidenceQuery request,
                                     CancellationToken cancellationToken)
    {
        return await _repository.GetRigIdsByMinerCoincidence(
            request.MinerName, request.MinerVersion, request.UserId, cancellationToken);
    }
}
