using MediatR;
using Microsoft.Extensions.Logging;
using MNX.Application.UseCases.CommandValidation;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.RigsApi.Core.ValueObjects;
using MNX.MonitoringCenter.RigsApi.GrainWrapper.Services;

namespace MNX.MonitoringCenter.RigsApi.UseCases.RigState.Power;

/// <summary>
/// Команда включения рига.
/// </summary>
/// <param name="RigId"> Идентификатор рига. </param>
public record TurnOnCommand(RigId RigId) : IValidatableCommand<Unit>;


/// <summary>
/// Обработчик <see cref="TurnOnCommand"/>.
/// </summary>
public class TurnOnCommandHandler :
    BaseCommandHandler,
    IRequestHandler<TurnOnCommand, Result<Unit>>
{
    private readonly IRigGrainFactory _rigGrainFactory;

    public TurnOnCommandHandler(IMediator mediator,
                                  ILogger<TurnOnCommandHandler> logger,
                                  IRigGrainFactory rigGrainFactory)
        : base(mediator, logger)
    {
        _rigGrainFactory = rigGrainFactory ?? throw new ArgumentNullException(nameof(rigGrainFactory));
    }

    public async Task<Result<Unit>> Handle(TurnOnCommand request, CancellationToken cancellationToken)
    {
        var rig = await _rigGrainFactory.GetGrain(request.RigId, cancellationToken);

        if (rig is null)
        {
            return Result<Unit>.Invalid($"Rig with id equaled {request.RigId} wasn't found");
        }

        var result = await rig.TurnOn();

        if (!await HandleResult(result, cancellationToken))
        {
            return Result<Unit>.Error("Failed to turn on rig");
        }

        return Result<Unit>.Empty();
    }
}
