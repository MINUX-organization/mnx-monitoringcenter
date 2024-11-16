using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs;

namespace MNX.MonitoringCenter.Inventory.UseCases.RigInventory;

/// <summary>
/// Обработчик <see cref="SaveRigInventoryCommand"/>.
/// </summary>
public class SaveRigInventoryCommandHandler : IRequestHandler<SaveRigInventoryCommand, Result<Unit>>
{
    private readonly IRigRepository _repository;

    public SaveRigInventoryCommandHandler(IRigRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Result<Unit>> Handle(SaveRigInventoryCommand request, CancellationToken cancellationToken)
    {
        await _repository.SaveInventory(request.Message.RigId, request.Message.CreatedDateTime,
                                        request.Message.Inventory, cancellationToken);

        return Result<Unit>.Empty();
    }
}
