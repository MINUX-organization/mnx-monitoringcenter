using MediatR;
using Microsoft.Extensions.Logging;
using MNX.Application.UseCases.CommandValidation;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.RigsApi.Core.ValueObjects;
using MNX.MonitoringCenter.RigsApi.GrainWrapper.Services;

namespace MNX.MonitoringCenter.RigsApi.UseCases.RigState;

/// <summary>
/// Команда на установку инвентаризации на риг.
/// </summary>
/// <param name="RigId"> Идентификатор рига. </param>
/// <param name="InventoryId"> Идентификатор инвентаризации. </param>
public record SetInventoryCommand(RigId RigId, long InventoryId) : IValidatableCommand<Unit>;


/// <summary>
/// Обработчик <see cref="SetInventoryCommand"/>.
/// </summary>
public class SetInventoryCommandHandler :
    BaseCommandHandler,
    IRequestHandler<SetInventoryCommand, Result<Unit>>
{
    private readonly IRigGrainFactory _rigGrainFactory;

    public SetInventoryCommandHandler(IMediator mediator,
                                      ILogger<SetInventoryCommandHandler> logger,
                                      IRigGrainFactory rigGrainFactory)
        : base(mediator, logger)
    {
        _rigGrainFactory = rigGrainFactory
            ?? throw new ArgumentNullException(nameof(rigGrainFactory));
    }

    public async Task<Result<Unit>> Handle(SetInventoryCommand request, CancellationToken cancellationToken)
    {
        var rig = await _rigGrainFactory.GetGrain(request.RigId, cancellationToken);

        if (rig is null)
        {
            return Result<Unit>.Invalid($"Rig with id equaled {request.RigId} wasn't found");
        }

        var result = await rig.SetInventory(request.InventoryId);

        if (!await HandleResult(result, cancellationToken))
        {
            return Result<Unit>.Error("Failed to set rig inventory");
        }

        return Result<Unit>.Empty();
    }
}
