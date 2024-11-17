using MediatR;
using MNX.MonitoringCenter.Inventory.Contracts;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs;
using System.Runtime.CompilerServices;

namespace MNX.MonitoringCenter.Inventory.UseCases;

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

    public async IAsyncEnumerable<Rig> Handle(GetRigsQuery request,
                                             [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var rigs = _repository.GetRigs(request.Specification);

        await foreach (var rig in rigs.WithCancellation(cancellationToken))
        {
            yield return new Rig()
            {
                Id = rig.Id,
                OwnerId = rig.OwnerId,
                Name = rig.Name
            };
        }
    }
}
