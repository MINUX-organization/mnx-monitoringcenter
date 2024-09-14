using MNX.Application.UseCases;
using MNX.MonitoringCenter.Management.UseCases.FlightSheets.Commands.Models;

namespace MNX.MonitoringCenter.Management.UseCases.FlightSheets.Commands.CreateFlightSheet;

/// <summary>
/// Команда добавления полётного листа.
/// </summary>
public sealed class CreateFlightSheetCommand : IValidatableCommand<Guid>
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// Модель полётного листа.
    /// </summary>
    public FlightSheetInputModel Model { get; }

    public CreateFlightSheetCommand(Guid userId, FlightSheetInputModel model)
    {
        UserId = userId;
        Model = model;
    }
}
