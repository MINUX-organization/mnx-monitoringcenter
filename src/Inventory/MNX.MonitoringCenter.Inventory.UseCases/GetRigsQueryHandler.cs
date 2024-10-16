using MediatR;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs;

namespace MNX.MonitoringCenter.Inventory.UseCases;

using Rig = Contracts.Rig;

/// <summary>
/// Обработчик <see cref="GetRigsQuery"/>.
/// </summary>
public class GetRigsQueryHandler : IStreamRequestHandler<GetRigsQuery, Rig>
{
    private readonly IRigRepository _repository;

    public GetRigsQueryHandler(IRigRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public IAsyncEnumerable<Rig> Handle(GetRigsQuery request, CancellationToken cancellationToken)
    {
        return _repository.GetRigs(request.Specification);
    }
}
