using MediatR;
using MNX.Application.UseCases.CommandValidation;
using MNX.Application.UseCases.Results;
using MNX.RigCommander.MessageQueue.Clients.Bus;

namespace MNX.MonitoringCenter.Management.UseCases.Mining;

/// <summary>
/// Команда запуска майнига.
/// </summary>
/// <param name="RigId"> Идентификатор рига. </param>
/// <param name="UserId"> Идентификатор пользователя. </param>
public sealed record StartMiningCommand(Guid RigId, Guid UserId) : IValidatableCommand<Unit>;


/// <summary>
/// Обработчик <see cref="StartMiningCommand"/>.
/// </summary>
public class StartMiningCommandHandler : IRequestHandler<StartMiningCommand, Result<Unit>>
{
    private readonly IRigRepository _repository;

    private readonly IQueueBusClient _messageQueueClient;

    public StartMiningCommandHandler(IRigRepository rigRepository,
                                     IQueueBusClient messageQueueClient)
    {
        _repository = rigRepository
            ?? throw new ArgumentNullException(nameof(rigRepository));

        _messageQueueClient = messageQueueClient
            ?? throw new ArgumentNullException(nameof(messageQueueClient));
    }

    public async Task<Result<Unit>> Handle(StartMiningCommand request, CancellationToken cancellationToken)
    {
        if (!await _repository.Exists(request.RigId, request.UserId))
        {
            return Result<Unit>.Invalid("Rig was not found");
        }

        await _messageQueueClient.Enqueue(
            new Agent.Commands.Mining.StartMiningCommand(),
            new Guid[] { request.RigId },
            request.UserId,
            cancellationToken: cancellationToken);

        return Result<Unit>.Empty();
    }
}
