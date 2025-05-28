using MediatR;
using MNX.Application.UseCases.Results;
using MNX.RigCommander.MessageQueue.Clients.Bus;
using MNX.Application.UseCases.CommandValidation;

namespace MNX.MonitoringCenter.Management.UseCases;

/// <summary>
/// Команда перезагрузка рига.
/// </summary>
/// <param name="RigId"> Идентификатор рига. </param>
/// <param name="UserId"> Идентификатор пользователя. </param>
public sealed record RebootRigCommand(Guid RigId, Guid UserId) : IValidatableCommand<Unit>;


/// <summary>
/// Обработчик <see cref="RebootRigCommand"/>.
/// </summary>
public class RebootRigCommandHandler : IRequestHandler<RebootRigCommand, Result<Unit>>
{
    private readonly IRigRepository _repository;

    private readonly IQueueBusClient _messageQueueClient;

    public RebootRigCommandHandler(IRigRepository rigRepository,
                                   IQueueBusClient messageQueueClient)
    {
        _repository = rigRepository
            ?? throw new ArgumentNullException(nameof(rigRepository));

        _messageQueueClient = messageQueueClient
            ?? throw new ArgumentNullException(nameof(messageQueueClient));
    }

    public async Task<Result<Unit>> Handle(RebootRigCommand request, CancellationToken cancellationToken)
    {
        if (!await _repository.Exists(request.RigId, request.UserId))
        {
            return Result<Unit>.Invalid("Rig was not found");
        }

        await _messageQueueClient.Enqueue(
            new Agent.Commands.RebootCommand(),
            new Guid[] { request.RigId },
            request.UserId,
            cancellationToken: cancellationToken);

        return Result<Unit>.Empty();
    }
}
