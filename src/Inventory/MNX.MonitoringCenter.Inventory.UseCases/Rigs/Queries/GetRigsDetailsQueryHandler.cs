using MediatR;
using MNX.MonitoringCenter.Inventory.Contracts;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs;

namespace MNX.MonitoringCenter.Inventory.UseCases.Rigs.Queries;

/// <summary>
/// Обработчик <see cref="GetRigsDetailsQuery"/>.
/// </summary>
public class GetRigsDetailsQueryHandler : IStreamRequestHandler<GetRigsDetailsQuery, RigDetails>
{
    private readonly IRigRepository _repository;

    public GetRigsDetailsQueryHandler(IRigRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public IAsyncEnumerable<RigDetails> Handle(GetRigsDetailsQuery request, CancellationToken cancellationToken)
    {
        return _repository.GetRigs(request.Specification);
    }
}
