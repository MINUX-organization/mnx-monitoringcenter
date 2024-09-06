using MediatR;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Queries.GetRigsInformation;

/// <summary>
/// Запрос на получение информации о ригах.
/// </summary>
public class GetRigsInformationQuery : IStreamRequest<RigInformationMessage>
{
    /// <summary>
    /// Спецификация.
    /// </summary>
    public Specification Specification { get; }

    public GetRigsInformationQuery(Guid userId,
                                   string? searchString = null,
                                   string? filterString = null,
                                   string[]? filterArguments = null)
    {
        Specification = new Specification(userId, searchString, filterString, filterArguments);
    }
}
