using EasyNetQ;
using MediatR;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Monitoring.Contracts.Bus.Messages;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Commands.Devices.Gpu.SetOverclocking;

/// <summary>
/// Обработчик команды установки разгона видеокарте.
/// </summary>
public class SetGpuOverclockingCommandHandler : IRequestHandler<SetGpuOverclockingCommand, Result<Unit>>
{
    private readonly IPubSub _pubSub;

    public SetGpuOverclockingCommandHandler(IPubSub pubSub)
    {
        _pubSub = pubSub ?? throw new ArgumentNullException(nameof(pubSub));
    }

    public async Task<Result<Unit>> Handle(SetGpuOverclockingCommand request, CancellationToken cancellationToken)
    {
        await _pubSub.PublishAsync(new OverclockingSettingWaitingMessage()
        {
            UserId = request.UserId,
            ConnectionId = request.ConnectionId,
            CardId = request.CardId,
            Overclocking = request.Overclocking
        }, cancellationToken: cancellationToken);

        return Result<Unit>.Empty();
    }
}
