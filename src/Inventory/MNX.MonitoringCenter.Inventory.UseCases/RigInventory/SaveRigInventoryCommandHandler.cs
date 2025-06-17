using MediatR;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs;

namespace MNX.MonitoringCenter.Inventory.UseCases.RigInventory;

/// <summary>
/// Обработчик <see cref="SaveRigInventoryCommand"/>.
/// </summary>
public class SaveRigInventoryCommandHandler : IRequestHandler<SaveRigInventoryCommand, Result<Unit>>
{
    private readonly IMediator _mediator;

    private readonly IRigRepository _repository;

    public SaveRigInventoryCommandHandler(IMediator mediator, IRigRepository repository)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Result<Unit>> Handle(SaveRigInventoryCommand request, CancellationToken cancellationToken)
    {
        var inventoryId = await _repository.SaveInventory(request.Message.RigId, request.Message.CreatedDateTime,
                                        request.Message.Inventory, cancellationToken);

        await _mediator.Publish(new RigInventorySavedEvent(request.Message, inventoryId), cancellationToken);

        return Result<Unit>.Empty();
    }
}
