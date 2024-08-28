using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Inventory.Contracts;

namespace MNX.MonitoringCenter.Inventory.UseCases;

/// <summary>
/// Команда сохранения инвентаризации.
/// </summary>
/// <param name="Message"> Сообщение с инвентаризацией. </param>
public sealed record SaveInventoryCommand(InventoryMsg Message) : IValidatableCommand<Unit>;

/// <summary>
/// Обработчик <see cref="SaveInventoryCommand"/>.
/// </summary>
public class SaveInventoryCommandHandler : IRequestHandler<SaveInventoryCommand, Result<Unit>>
{
    private readonly IInventoryRepository _repository;

    public SaveInventoryCommandHandler(IInventoryRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Result<Unit>> Handle(SaveInventoryCommand request, CancellationToken cancellationToken)
    {
        await _repository.Save(request.Message.RigId, request.Message.CreatedDateTime,
                               request.Message.Inventory, cancellationToken);

        return Result<Unit>.Empty();
    }
}
