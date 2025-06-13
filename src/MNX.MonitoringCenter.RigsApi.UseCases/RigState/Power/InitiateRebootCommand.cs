using MediatR;
using Microsoft.Extensions.Logging;
using MNX.Application.UseCases.Requests;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.RigsApi.Core.ValueObjects;
using MNX.MonitoringCenter.RigsApi.GrainWrapper.Services;

namespace MNX.MonitoringCenter.RigsApi.UseCases.RigState.Power;

/// <summary>
/// Команда перезагрузки рига.
/// </summary>
/// <param name="RigId"> Идентификатор рига. </param>
/// <param name="UserId"> Идентификатор пользователя. </param>
public record InitiateRebootCommand(RigId RigId, Guid UserId) : IUserableValidatableCommand<Unit>;


/// <summary>
/// Обработчик <see cref="InitiateRebootCommand"/>.
/// </summary>
public class InitiateRebootCommandHandler :
    BaseCommandHandler,
    IRequestHandler<InitiateRebootCommand, Result<Unit>>
{
    private readonly IRigGrainFactory _rigGrainFactory;

    public InitiateRebootCommandHandler(IMediator mediator,
                                        ILogger<InitiateRebootCommandHandler> logger,
                                        IRigGrainFactory rigGrainFactory)
        : base(mediator, logger)
    {
        _rigGrainFactory = rigGrainFactory ?? throw new ArgumentNullException(nameof(rigGrainFactory));
    }

    public async Task<Result<Unit>> Handle(InitiateRebootCommand request, CancellationToken cancellationToken)
    {
        var rig = await _rigGrainFactory.GetGrain(request.RigId, cancellationToken);

        if (rig is null)
        {
            return Result<Unit>.Invalid($"Rig with id equaled {request.RigId} wasn't found");
        }

        var result = await rig.InitiateReboot(request.RigId);

        if (!await HandleResult(result, cancellationToken))
        {
            return Result<Unit>.Error("Failed to initiate rig reboot");
        }

        return Result<Unit>.Empty();
    }
}
