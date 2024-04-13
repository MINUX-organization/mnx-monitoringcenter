using MediatR;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Queries.GetRigsInformation;

/// <summary>
/// Запрос на получение информации о ригах.
/// </summary>
/// <param name="specification"> Спецификация. </param>
public record GetRigsInformationQuery(Specification specification) : IRequest<GetRigsInformationResult>;
