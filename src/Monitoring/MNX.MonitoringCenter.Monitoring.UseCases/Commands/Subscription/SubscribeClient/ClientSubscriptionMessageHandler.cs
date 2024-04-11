using AutoMapper;
using EasyNetQ;
using MediatR;
using MNX.MonitoringCenter.Monitoring.Contracts.Enums;
using MNX.MonitoringCenter.Monitoring.Contracts.Messages.Bus;
using MNX.MonitoringCenter.Monitoring.Core;
using MNX.MonitoringCenter.Monitoring.UseCases.Abstractions;
using MNX.MonitoringCenter.Monitoring.UseCases.Commands.Subscription.SubscribeClient.Models;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Commands.Subscription.SubscribeClient;

/// <summary>
/// Обработчик команды сообщения о подписки клиента.
/// </summary>
public class ClientSubscriptionMessageHandler : IRequestHandler<ClientSubscriptionMessage, SubscribeClientResult>
{
    private readonly IRigRepository _rigRepository;

    private readonly IPubSub _pubSub;

    private readonly IMapper _mapper;

    public ClientSubscriptionMessageHandler(IPubSub pubSub,
                                            IRigRepository rigRepository,
                                            IMapper mapper)
    {
        _pubSub = pubSub;
        _rigRepository = rigRepository;
        _mapper = mapper;
    }

    public async Task<SubscribeClientResult> Handle(ClientSubscriptionMessage request,
                                                    CancellationToken cancellationToken)
    {
        await SendMessagesToBus(request, cancellationToken);

        var rigs = await _rigRepository.GetAvailable(request.SubscriptionModel.UserId);

        return ToResult(rigs);
    }

    /// <summary>
    /// Отправить сообщения в шину.
    /// </summary>
    /// <param name="message"> Команда подписки клиента. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    private async Task SendMessagesToBus(ClientSubscriptionMessage message,
                                         CancellationToken cancellationToken)
    {
        await _pubSub.PublishAsync(new RigsStateWaitingMessage
        {
            UserId = message.SubscriptionModel.UserId,
            ConnectionId = message.SubscriptionModel.ConnectionId
        }, cancellationToken: cancellationToken);

        if (message.SubscribersCount == 1)
        {
            await _pubSub.PublishAsync(new StatisticsStreamStartCommand
            {
                UserId = message.SubscriptionModel.UserId,
                ObservableObjectsType = ObservableObjectsType.Rigs
            }, cancellationToken: cancellationToken);
        }
    }

    /// <summary>
    /// Собрать результат подписки клиента.
    /// </summary>
    /// <param name="rigs"> Коллекция ригов. </param>
    /// <returns> Результат подписки клиента. </returns>
    private SubscribeClientResult ToResult(IEnumerable<Rig> rigs)
    {
        var rigsInformation = _mapper.Map<IEnumerable<RigInformationMessage>>(rigs);

        for (int i = 0; i < rigsInformation.Count(); i++)
        {
            rigsInformation.ElementAt(i).Index = i + 1;
        }

        return new SubscribeClientResult()
        {
            Rigs = rigsInformation,

            TotalCpusCount = new TotalCpusCount()
            {
                Total = rigs.Sum(x => x.TotalCpusCount),
                Amd = rigs.Sum(x => x.AmdCpusCount),
                Intel = rigs.Sum(x => x.IntelCpusCount)
            },

            TotalGpusCount = new TotalGpusCount()
            {
                Total = rigs.Sum(x => x.TotalGpusCount),
                Nvidia = rigs.Sum(x => x.NvidiaGpusCount),
                Amd = rigs.Sum(x => x.AmdGpusCount),
                Intel = rigs.Sum(x => x.IntelGpusCount)
            }
        };
    }
}
