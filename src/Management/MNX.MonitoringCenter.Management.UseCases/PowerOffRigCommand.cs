using MediatR;
using MNX.Application.UseCases.CommandValidation;
using MNX.Application.UseCases.Results;
using MNX.RigCommander.MessageQueue.Clients.Bus;

namespace MNX.MonitoringCenter.Management.UseCases;

/// <summary>
/// Команда выключения рига.
/// </summary>
/// <param name="RigId"> Идентификатор рига. </param>
/// <param name="UserId"> Идентификатор пользователя. </param>
public sealed record PowerOffRigCommand(Guid RigId, Guid UserId) : IValidatableCommand<Unit>;


/// <summary>
/// Обработчик <see cref="PowerOffRigCommand"/>.
/// </summary>
public class PowerOffRigCommandHandler : IRequestHandler<PowerOffRigCommand, Result<Unit>>
{
    private readonly IRigRepository _repository;

    private readonly IQueueBusClient _messageQueueClient;

    public PowerOffRigCommandHandler(IRigRepository rigRepository,
                                     IQueueBusClient messageQueueClient)
    {
        _repository = rigRepository
            ?? throw new ArgumentNullException(nameof(rigRepository));

        _messageQueueClient = messageQueueClient
            ?? throw new ArgumentNullException(nameof(messageQueueClient));
    }

    public async Task<Result<Unit>> Handle(PowerOffRigCommand request, CancellationToken cancellationToken)
    {
        if (!await _repository.Exists(request.RigId, request.UserId))
        {
            return Result<Unit>.Invalid("Rig was not found");
        }

        await _messageQueueClient.Enqueue(
            new Agent.Commands.PowerOffCommand(),
            new Guid[] { request.RigId },
            request.UserId,
            cancellationToken: cancellationToken);

        return Result<Unit>.Empty();
    }
}
