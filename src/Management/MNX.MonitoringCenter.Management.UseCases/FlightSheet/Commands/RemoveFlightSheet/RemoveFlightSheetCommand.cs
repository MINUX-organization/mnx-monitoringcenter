using MediatR;
using MNX.Application.UseCases;

namespace MNX.MonitoringCenter.Management.UseCases.FlightSheets.Commands.RemoveFlightSheet;

/// <summary>
/// Команда удаления полётного листа.
/// </summary>
/// <param name="Id"> Идентификатор. </param>
public sealed record RemoveFlightSheetCommand(Guid Id, Guid UserId) : IRequest<Result<Unit>>;
