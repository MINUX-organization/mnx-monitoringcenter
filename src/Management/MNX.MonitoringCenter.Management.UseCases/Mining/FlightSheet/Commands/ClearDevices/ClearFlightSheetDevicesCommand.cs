using MediatR;
using MNX.Application.UseCases.Results;
using MNX.Application.UseCases.Requests;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.ClearDevices;

/// <summary>
/// Команда на снятие полётного листа со всех устройств.
/// </summary>
/// <param name="FlightSheetId"> Идентификатор полётного листа. </param>
/// <param name="UserId"> Идентификатор пользователя. </param>
public sealed record ClearFlightSheetDevicesCommand(Guid FlightSheetId, Guid UserId)
    : IUserableRequest<Result<Unit>>;
