using MediatR;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Queries.GetRigsIds;

/// <summary>
/// Запрос на получение списка идентификаторов ригов.
/// </summary>
public class GetRigsIdsQuery : IRequest<IEnumerable<Guid>>
{
    /// <summary>
    /// Спецификация.
    /// </summary>
    public Specification Specification { get; }

    public GetRigsIdsQuery(Specification specification)
    {
        Specification = specification;
    }
}
