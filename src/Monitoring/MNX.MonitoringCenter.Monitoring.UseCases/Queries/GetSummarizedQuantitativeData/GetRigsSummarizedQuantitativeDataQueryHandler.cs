using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Monitoring.Core;
using MNX.MonitoringCenter.Monitoring.UseCases.Abstractions;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Queries.GetSummarizedQuantitativeData;

/// <summary>
/// Обработчик запроса на получение обобщённых количественных данных ригов.
/// </summary>
public class GetRigsSummarizedQuantitativeDataQueryHandler :
    IRequestHandler<GetRigsSummarizedQuantitativeDataQuery, Result<RigsSummarizedQuantitativeData>>
{
    /// <summary>
    /// Репозиторий для доступа к ригам.
    /// </summary>
    private readonly IRigRepository _rigRepository;

    public GetRigsSummarizedQuantitativeDataQueryHandler(IRigRepository rigRepository)
    {
        _rigRepository = rigRepository ?? throw new ArgumentNullException(nameof(rigRepository));
    }

    public async Task<Result<RigsSummarizedQuantitativeData>> Handle(
        GetRigsSummarizedQuantitativeDataQuery request, CancellationToken cancellationToken)
    {
        var totalData = await _rigRepository.GetRigsSummarizedQuantitativeData(request.Specification);
        return Result<RigsSummarizedQuantitativeData>.Success(totalData);
    }
}
