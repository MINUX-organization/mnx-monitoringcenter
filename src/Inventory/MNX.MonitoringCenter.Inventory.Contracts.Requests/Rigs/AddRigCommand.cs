using MediatR;
using MNX.Application.UseCases.CommandValidation;

namespace MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs;

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

    /// <summary>
    /// Имя Агента.
    /// </summary>
    public string Name { get; init; }

    public AddRigCommand(Guid id, Guid ownerId, string name)
    {
        Id = id;
        OwnerId = ownerId;
        Name = name;
    }
}
