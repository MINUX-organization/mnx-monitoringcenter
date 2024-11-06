using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Inventory.Contracts.RigInventory;

namespace MNX.MonitoringCenter.Inventory.UseCases.RigInventory;

/// <summary>
/// Команда сохранения инвентаризации рига.
/// </summary>
/// <param name="Message"> Сообщение с инвентаризацией. </param>
public sealed record SaveRigInventoryCommand(RigInventoryMsg Message) : IValidatableCommand<Unit>;

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
