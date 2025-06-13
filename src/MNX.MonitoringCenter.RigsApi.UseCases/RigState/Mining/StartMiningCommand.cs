using MediatR;
using Microsoft.Extensions.Logging;
using MNX.Application.UseCases.CommandValidation;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.RigsApi.Core.ValueObjects;
using MNX.MonitoringCenter.RigsApi.GrainWrapper.Services;

namespace MNX.MonitoringCenter.RigsApi.UseCases.RigState.Mining;

/// <summary>
/// Команда запуска майнинга.
/// </summary>
/// <param name="RigId"> Идентификатор рига. </param>
public record StartMiningCommand(RigId RigId) : IValidatableCommand<Unit>;


/// <summary>
/// Обработчик <see cref="StartMiningCommand"/>.
/// </summary>
public class StartMiningCommandHandler :
    BaseCommandHandler,
    IRequestHandler<StartMiningCommand, Result<Unit>>
{
    private readonly IRigGrainFactory _rigGrainFactory;

    public StartMiningCommandHandler(IMediator mediator,
                                     ILogger<StartMiningCommandHandler> logger,
                                     IRigGrainFactory rigGrainFactory)
        : base(mediator, logger)
    {
        _rigGrainFactory = rigGrainFactory ?? throw new ArgumentNullException(nameof(rigGrainFactory));
    }

    public async Task<Result<Unit>> Handle(StartMiningCommand request, CancellationToken cancellationToken)
    {
        var rig = await _rigGrainFactory.GetGrain(request.RigId, cancellationToken);

        if (rig is null)
        {
            return Result<Unit>.Invalid($"Rig with id equaled {request.RigId} wasn't found");
        }

        var result = await rig.StartMining();

        if (!await HandleResult(result, cancellationToken))
        {
            return Result<Unit>.Error("Failed to mining start to rig");
        }

        return Result<Unit>.Empty();
    }
}
