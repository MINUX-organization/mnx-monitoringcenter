using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Monitoring.Core;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Queries.GetSummarizedQuantitativeData;

/// <summary>
/// Запрос на получение обобщённых количественных данных ригов.
/// </summary>
/// <param name="Specification"> Спецификация. </param>
public record GetRigsSummarizedQuantitativeDataQuery
    : IRequest<Result<RigsSummarizedQuantitativeData>>
{
    /// <summary>
    /// Спецификация.
    /// </summary>
    public Specification Specification { get; }

    public GetRigsSummarizedQuantitativeDataQuery(Guid userId)
    {
        Specification = new Specification(userId);
    }
}
