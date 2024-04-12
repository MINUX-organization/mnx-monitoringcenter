using MediatR;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Queries.GetRigsInformation;

/// <summary>
/// Запрос на получение информации о ригах.
/// </summary>
/// <param name="UserId"> Идентификатор пользователя. </param>
public record GetRigsInformationQuery(long UserId) : IRequest<GetRigsInformationResult>;
