using MediatR;
using MNX.Application.UseCases;

namespace MNX.MonitoringCenter.Inventory.Contracts.Requests.Rig;

/// <summary>
/// Команда на добавление рига.
/// </summary>
public class AddRigCommand : IValidatableCommand<Unit>
{
    /// <summary>
    /// Уникальный идентификатор рига.
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// Уникальный идентификатор владельца рига.
    /// </summary>
    public Guid OwnerId { get; init; }

    public AddRigCommand(Guid id, Guid ownerId)
    {
        Id = id;
        OwnerId = ownerId;
    }
}
