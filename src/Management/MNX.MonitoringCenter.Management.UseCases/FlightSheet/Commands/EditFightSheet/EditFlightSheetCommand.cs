using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Management.UseCases.FlightSheets.Commands.Models;

namespace MNX.MonitoringCenter.Management.UseCases.FlightSheets.Commands.EditFightSheet;

/// <summary>
/// Команды обновления полётного листа.
/// </summary>
public sealed class EditFlightSheetCommand : IValidatableCommand<Unit>
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// Модель полётного листа.
    /// </summary>
    public FlightSheetInputModel Model { get; }

    public EditFlightSheetCommand(Guid id, Guid userId, FlightSheetInputModel model)
    {
        Id = id;
        UserId = userId;
        Model = model;
    }
}
