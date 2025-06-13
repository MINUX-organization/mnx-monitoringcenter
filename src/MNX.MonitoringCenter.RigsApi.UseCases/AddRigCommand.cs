using MediatR;
using MNX.Application.UseCases.CommandValidation;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.RigsApi.Core;
using MNX.MonitoringCenter.RigsApi.Core.Services;
using MNX.MonitoringCenter.RigsApi.Core.ValueObjects;

namespace MNX.MonitoringCenter.RigsApi.UseCases;

/// <summary>
/// Команда на добавление рига.
/// </summary>
public class AddRigCommand : IValidatableCommand<Unit>
{
    /// <summary>
    /// Уникальный идентификатор рига.
    /// </summary>
    public RigId Id { get; init; }

    /// <summary>
    /// Уникальный идентификатор владельца рига.
    /// </summary>
    public Guid OwnerId { get; init; }

    /// <summary>
    /// Имя Агента.
    /// </summary>
    public string Name { get; init; }

    public AddRigCommand(RigId id, Guid ownerId, string name)
    {
        Id = id;
        OwnerId = ownerId;
        Name = name;
    }
}


/// <summary>
/// Обработчик <see cref="AddRigCommand"/>.
/// </summary>
public class AddRigCommandHandler : IRequestHandler<AddRigCommand, Result<Unit>>
{
    private readonly IRigRepository _repository;

    public AddRigCommandHandler(IRigRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Result<Unit>> Handle(AddRigCommand request, CancellationToken cancellationToken)
    {
        await _repository.Add(new Rig(request.Id, request.OwnerId, request.Name), cancellationToken);
        return Result<Unit>.Empty();
    }
}
