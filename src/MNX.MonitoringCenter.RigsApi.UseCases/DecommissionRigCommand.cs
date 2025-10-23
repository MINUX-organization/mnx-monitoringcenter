using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.Extensions.Logging;

using MNX.Application.UseCases.Results;
using MNX.Application.UseCases.Requests;

using MNX.MonitoringCenter.RigsApi.Core.ValueObjects;
using MNX.MonitoringCenter.RigsApi.GrainWrapper.Services;
using MNX.MonitoringCenter.RigsApi.UseCases.RigState;

namespace MNX.MonitoringCenter.RigsApi.UseCases;

/// <summary>
/// Команда выведения рига из строя (мягкое удаление).
/// </summary>
/// <param name="RigId"> Идентификатор рига. </param>
/// <param name="UserId"> Идентификатор пользователя. </param>
public sealed record DecommissionRigCommand(RigId RigId, Guid UserId) 
    : IUserable, IRequest<Result<Unit>>;

/// <summary>
/// Обработчик команды <see cref="DecommissionRigCommand"/>.
/// </summary>
public sealed class DecommissionRigCommandHandler : 
    BaseCommandHandler,
    IRequestHandler<DecommissionRigCommand, Result<Unit>>
{
    private readonly IRigGrainFactory _rigGrainFactory;

    public DecommissionRigCommandHandler(IMediator mediator, ILogger<BaseCommandHandler> logger, IRigGrainFactory rigGrainFactory) : base(mediator, logger)
    {
        _rigGrainFactory = rigGrainFactory;
    }

    /// <inheritdoc />
    public async Task<Result<Unit>> Handle(DecommissionRigCommand request, CancellationToken cancellationToken)
    {
        var rig = await _rigGrainFactory.GetGrain(request.RigId, cancellationToken);
        if (rig is null || rig.IsDecommissioned)
        {
            return Result<Unit>.Empty();
        }

        if (rig.OwnerId != request.UserId)
        {
            return Result<Unit>.Invalid($"Rig with id equaled {request.RigId} wasn't found");
        }

        var result = await rig.Decommission();
        if (!await HandleResult(result, cancellationToken))
        {
            return Result<Unit>.Error("Failed to decommission rig!"); 
        }
        
        return Result<Unit>.Empty();
    }
}