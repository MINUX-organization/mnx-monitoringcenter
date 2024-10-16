using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rig;
using MNX.MonitoringCenter.Inventory.Contracts.Rig;

namespace MNX.MonitoringCenter.Inventory.UseCases.Rigs;

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
        await _repository.Add(new Rig()
        {
            Id = request.Id,
            OwnerId = request.OwnerId,
            Name = "Minux"
        },cancellationToken);

        return Result<Unit>.Empty();
    }
}
