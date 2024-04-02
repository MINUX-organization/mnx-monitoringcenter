using AutoMapper;
using EasyNetQ;
using MediatR;
using MNX.MonitoringCenter.Monitoring.Contracts.Enums;
using MNX.MonitoringCenter.Monitoring.Contracts.Messages;
using MNX.MonitoringCenter.Monitoring.Contracts.Models;
using MNX.MonitoringCenter.Monitoring.UseCases.Abstractions;
using System.Runtime.CompilerServices;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Commands.SubscribeClient;

/// <summary>
/// Обработчик команды подписки клиента.
/// </summary>
public class SubscribeClientCommandHandler : IStreamRequestHandler<SubscribeClientCommand, RigInformation>
{
    private readonly IRigRepository _rigRepository;

    private readonly IPubSub _pubSub;

    private readonly IMapper _mapper;

    private readonly ConnectionCounter _connectionCounter;

    public SubscribeClientCommandHandler(IPubSub pubSub,
                                         IRigRepository rigRepository,
                                         IMapper mapper,
                                         ConnectionCounter connectionCounter)
    {
        _pubSub = pubSub;
        _rigRepository = rigRepository;
        _mapper = mapper;
        _connectionCounter = connectionCounter;
    }

    public async IAsyncEnumerable<RigInformation> Handle(SubscribeClientCommand request,
                                                         [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        int count = _connectionCounter.AddConnectionInGroup(request.ClientModel.UserId);

        await _pubSub.PublishAsync(new RigsStateWaitingMessage
        {
            UserId = request.ClientModel.UserId,
            ConnectionId = request.ClientModel.ConnectionId
        }, cancellationToken: cancellationToken);

        if (count == 1)
        {
            await _pubSub.PublishAsync(new StatisticsStreamStartCommand
            {
                UserId = request.ClientModel.UserId,
                ObservableObjectsType = ObservableObjectsType.Rigs
            }, cancellationToken: cancellationToken);
        }

        var rigs = _rigRepository.GetAvailable(request.ClientModel.UserId);

        int counter = 0;
        await foreach (var rig in rigs)
        {
            counter++;
            var result = _mapper.Map<RigInformation>(rig);
            result.Index = counter;

            yield return result;
        }
    }
}
