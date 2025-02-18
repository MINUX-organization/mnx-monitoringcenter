using MediatR;
using MNX.Application.UseCases.Results;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.RemoveFlightSheet;

/// <summary>
/// Команда удаления полётного листа.
/// </summary>
/// <param name="Id"> Идентификатор. </param>
public sealed record RemoveFlightSheetCommand(Guid Id, Guid UserId) : IRequest<Result<Unit>>;
