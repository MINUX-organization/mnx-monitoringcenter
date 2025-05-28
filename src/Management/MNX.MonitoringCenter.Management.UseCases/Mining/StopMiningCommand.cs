using MediatR;
using MNX.Application.UseCases.Results;
using MNX.RigCommander.MessageQueue.Clients.Bus;
using MNX.Application.UseCases.CommandValidation;

namespace MNX.MonitoringCenter.Management.UseCases.Mining;

/// <summary>
/// Команда остановки майнига.
/// </summary>
/// <param name="RigId"> Идентификатор рига. </param>
/// <param name="UserId"> Идентификатор пользователя. </param>
public sealed record StopMiningCommand(Guid RigId, Guid UserId) : IValidatableCommand<Unit>;


/// <summary>
/// Обработчик <see cref="StopMiningCommand"/>.
/// </summary>
public class StopMiningCommandHandler : IRequestHandler<StopMiningCommand, Result<Unit>>
{
    private readonly IRigRepository _repository;

    private readonly IQueueBusClient _messageQueueClient;

    public StopMiningCommandHandler(IRigRepository rigRepository,
                                    IQueueBusClient messageQueueClient)
    {
        _repository = rigRepository
            ?? throw new ArgumentNullException(nameof(rigRepository));

        _messageQueueClient = messageQueueClient
            ?? throw new ArgumentNullException(nameof(messageQueueClient));
    }

    public async Task<Result<Unit>> Handle(StopMiningCommand request, CancellationToken cancellationToken)
    {
        if (!await _repository.Exists(request.RigId, request.UserId))
        {
            return Result<Unit>.Invalid("Rig was not found");
        }

        await _messageQueueClient.Enqueue(
            new Agent.Commands.Mining.StopMiningCommand(),
            new Guid[] { request.RigId },
            request.UserId,
            cancellationToken: cancellationToken);

        return Result<Unit>.Empty();
    }
}
