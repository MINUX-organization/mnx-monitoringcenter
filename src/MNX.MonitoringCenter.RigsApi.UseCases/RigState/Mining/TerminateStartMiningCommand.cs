using MediatR;
using Microsoft.Extensions.Logging;
using MNX.Application.UseCases.CommandValidation;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.RigsApi.Core.ValueObjects;
using MNX.MonitoringCenter.RigsApi.GrainWrapper.Services;

namespace MNX.MonitoringCenter.RigsApi.UseCases.RigState.Mining;

/// <summary>
/// Команда прерывания запуска майнинга.
/// </summary>
/// <param name="RigId"> Идентификатор рига. </param>
public record TerminateStartMiningCommand(RigId RigId) : IValidatableCommand<Unit>;


/// <summary>
/// Обработчик <see cref="TerminateStartMiningCommand"/>.
/// </summary>
public class TerminateStartMiningCommandHandler :
    BaseCommandHandler,
    IRequestHandler<TerminateStartMiningCommand, Result<Unit>>
{
    private readonly IRigGrainFactory _rigGrainFactory;

    public TerminateStartMiningCommandHandler(IMediator mediator,
                                              ILogger<TerminateStartMiningCommandHandler> logger,
                                              IRigGrainFactory rigGrainFactory)
        : base(mediator, logger)
    {
        _rigGrainFactory = rigGrainFactory ?? throw new ArgumentNullException(nameof(rigGrainFactory));
    }

    public async Task<Result<Unit>> Handle(TerminateStartMiningCommand request, CancellationToken cancellationToken)
    {
        var rig = await _rigGrainFactory.GetGrain(request.RigId, cancellationToken);

        if (rig is null)
        {
            return Result<Unit>.Invalid($"Rig with id equaled {request.RigId} wasn't found");
        }

        var result = await rig.TerminateStartMining();

        if (!await HandleResult(result, cancellationToken))
        {
            return Result<Unit>.Error("Failed to terminate of mining start");
        }

        return Result<Unit>.Empty();
    }
}
