using MediatR;
using Microsoft.Extensions.Logging;
using MNX.Application.UseCases.Requests;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.RigsApi.Core.ValueObjects;
using MNX.MonitoringCenter.RigsApi.GrainWrapper.Services;

namespace MNX.MonitoringCenter.RigsApi.UseCases.RigState.Power;

/// <summary>
/// Команда инициации выключения рига.
/// </summary>
/// <param name="RigId"> Идентификатор рига. </param>
/// <param name="UserId"> Идентификатор пользователя. </param>
public record InitiatePowerOffCommand(RigId RigId, Guid UserId) : IUserableValidatableCommand<Unit>;


/// <summary>
/// Обработчик <see cref="InitiatePowerOffCommand"/>.
/// </summary>
public class InitiatePowerOffCommandHandler :
    BaseCommandHandler,
    IRequestHandler<InitiatePowerOffCommand, Result<Unit>>
{
    private readonly IRigGrainFactory _rigGrainFactory;

    public InitiatePowerOffCommandHandler(IMediator mediator,
                                          ILogger<InitiatePowerOffCommandHandler> logger,
                                          IRigGrainFactory rigGrainFactory)
        : base(mediator, logger)
    {
        _rigGrainFactory = rigGrainFactory
            ?? throw new ArgumentNullException(nameof(rigGrainFactory));
    }

    public async Task<Result<Unit>> Handle(InitiatePowerOffCommand request, CancellationToken cancellationToken)
    {
        var rig = await _rigGrainFactory.GetGrain(request.RigId, cancellationToken);

        if (rig == null || rig.OwnerId != request.UserId)
        {
            return Result<Unit>.Invalid($"Rig with is equaled {request.RigId} wasn't found");
        }

        var result = await rig.InitiatePowerOff(request.UserId);

        if (!await HandleResult(result, cancellationToken))
        {
            return Result<Unit>.Error("Failed to initiate rig shutdown");
        }

        return Result<Unit>.Empty();
    }
}
