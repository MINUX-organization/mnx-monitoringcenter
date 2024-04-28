using MediatR;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Queries.GetRigsInformation;

/// <summary>
/// Запрос на получение информации о ригах.
/// </summary>
/// <param name="Specification"> Спецификация. </param>
public record GetRigsInformationQuery(Specification Specification) : IStreamRequest<RigInformationMessage>;
