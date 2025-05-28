using MediatR;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs;

namespace MNX.MonitoringCenter.Inventory.UseCases.Rigs.Queries;

/// <summary>
/// Обработчик команды <see cref="GetRigIdsWithoutMinerQuery"/>.
/// </summary>
public class GetRigIdsWithoutMinerQueryHandler : IRequestHandler<GetRigIdsWithoutMinerQuery, Guid[]>
{
    private readonly IRigRepository _repository;

    public GetRigIdsWithoutMinerQueryHandler(IRigRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public Task<Guid[]> Handle(GetRigIdsWithoutMinerQuery request, CancellationToken cancellationToken)
    {
        return _repository.GetRigIdsWithoutMiner(
            request.RigIds, request.UserId, request.MinerName, request.MinerVersion, cancellationToken);
    }
}
