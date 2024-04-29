using MediatR;
using MNX.MonitoringCenter.Monitoring.UseCases.Abstractions;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Queries.GetRigsIds;

/// <summary>
/// Обработчик запроса на получение списка идентификаторов ригов.
/// </summary>
public class GetRigsIdsQueryHandler : IRequestHandler<GetRigsIdsQuery, IEnumerable<Guid>>
{
    private readonly IRigRepository _repository;

    public GetRigsIdsQueryHandler(IRigRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<IEnumerable<Guid>> Handle(GetRigsIdsQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetIds(request.Specification);
    }
}
