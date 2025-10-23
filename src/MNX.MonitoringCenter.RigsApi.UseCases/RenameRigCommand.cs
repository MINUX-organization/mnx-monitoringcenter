using MediatR;
using Microsoft.Extensions.Logging;

using MNX.Application.UseCases.Results;
using MNX.Application.UseCases.CommandValidation;
using MNX.Application.UseCases.Requests;
using MNX.MonitoringCenter.RigsApi.Core.ValueObjects;
using MNX.MonitoringCenter.RigsApi.UseCases.RigState;
using MNX.MonitoringCenter.RigsApi.GrainWrapper.Services;

namespace MNX.MonitoringCenter.RigsApi.UseCases;

/// <summary>
/// Команда переименования рига.
/// </summary>
/// <param name="RigId"> Идентификатор рига. </param>
public sealed record RenameRigCommand(RigId RigId, Guid UserId, string NewName) 
    : IUserableValidatableCommand<Unit>;

/// <summary>
/// Обработчик команды <see cref="RenameRigCommand"/>.
/// </summary>
public sealed class RenameRigCommandHandler : 
    BaseCommandHandler, 
    IRequestHandler<RenameRigCommand, Result<Unit>>
{
    private readonly IRigGrainFactory _rigGrainFactory;     
    
    public RenameRigCommandHandler(IRigGrainFactory rigGrainFactory, 
                                   IMediator mediator, 
                                   ILogger<BaseCommandHandler> logger) 
        : base(mediator, logger)
    {
            _rigGrainFactory = rigGrainFactory
                ?? throw new ArgumentNullException(nameof(rigGrainFactory));
    }

    /// <inheritdoc/>
    public async Task<Result<Unit>> Handle(RenameRigCommand request, CancellationToken cancellationToken)
    { 
        var rig = await _rigGrainFactory.GetGrain(request.RigId, cancellationToken);
        if (rig is null || rig.OwnerId != request.UserId)
        {
            return Result<Unit>.Invalid($"Rig with id equaled {request.RigId} wasn't found");
        }
        
        var result = await rig.Rename(request.NewName);
        if (!await HandleResult(result, cancellationToken))
        {
            return Result<Unit>.Error("Failed to rename rig!"); 
        }
        
        return Result<Unit>.Empty();
    }
}