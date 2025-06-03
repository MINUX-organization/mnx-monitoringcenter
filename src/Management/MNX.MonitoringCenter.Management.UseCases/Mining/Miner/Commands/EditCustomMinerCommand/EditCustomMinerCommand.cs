using MediatR;
using MNX.Application.UseCases.Results;
using MNX.Application.UseCases.Requests;
using MNX.RigCommander.MessageQueue.Clients.Bus;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs;
using MNX.MonitoringCenter.Management.Agent.Commands.Mining;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.Models;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.EditMinerCommand;

using Miner = Core.Mining.Miner.Miner;
using MinerTypeEnum = Core.Mining.Miner.Enums.MinerTypeEnum;

/// <summary>
/// Команда редактирования майнера.
/// </summary>
/// <param name="Model"> Модель ввода майнера. </param>
/// <param name="MinerId"> Идентификатор майнера. </param>
/// <param name="UserId"> Идентификатор пользователя. </param>
public sealed record EditCustomMinerCommand(MinerInputModel Model, Guid MinerId, Guid UserId)
    : IUserableValidatableCommand<Unit>;

/// <summary>
/// Обработчик команды <see cref="EditCustomMinerCommand"/>.
/// </summary>
public class EditMinerCommandHandler : IRequestHandler<EditCustomMinerCommand, Result<Unit>>
{
    private readonly IMediator _mediator;

    private readonly IQueueBusClient _bus;

    private readonly IMinerRepository _minerRepository;

    public EditMinerCommandHandler(IMediator mediator, IQueueBusClient bus, IMinerRepository minerRepository)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        _minerRepository = minerRepository ?? throw new ArgumentNullException(nameof(minerRepository));
    }

    public async Task<Result<Unit>> Handle(EditCustomMinerCommand request, CancellationToken cancellationToken)
    {
        var newModel = request.Model;

        var model = await _minerRepository.GetMinerById(request.MinerId, cancellationToken);

        if (model is null)
        {
            return Result<Unit>.Invalid($"Miner with id equaled {request.MinerId} was not found");
        }

        if ((newModel.Name != model.Name || newModel.Version != model.Version) &&
            await _minerRepository.Exists(request.MinerId, request.UserId, newModel.Name, cancellationToken))
        {
            return Result<Unit>.Conflict($"Miner with name {newModel.Name} already exists");
        }

        var newMiner = new Miner()
        {
            Id = request.MinerId,
            Name = newModel.Name,
            Version = newModel.Version,
            Type = MinerTypeEnum.Custom,
            InstallationUrl = newModel.InstallationUrl,
            SupportedDevices = newModel.SupportedDevices,
            PoolTemplate = newModel.PoolTemplate,
            WalletWorkerTemplate = newModel.WalletWorkerTemplate,
            MiningMode = newModel.MiningMode,
            OwnerId = request.UserId
        };
        await _minerRepository.Edit(newMiner, cancellationToken);

        if (HasRelevantMinerChanges(newMiner, model))
        {
            await SendInstallCommand(newMiner, model, request.UserId, cancellationToken);
        }

        return Result<Unit>.Empty();
    }

    /// <summary>
    /// Получить признак изменения параметров майнера.
    /// </summary>
    /// <param name="newMiner"> Новый майнер. </param>
    /// <param name="oldMiner"> Старый майнер. </param>
    /// <returns> Признак дифференциации майнеров. </returns>
    private bool HasRelevantMinerChanges(Miner newMiner, Miner oldMiner)
    {
        return newMiner.Name != oldMiner.Name ||
            newMiner.Version != oldMiner.Version ||
            newMiner.InstallationUrl != oldMiner.InstallationUrl ||
            newMiner.PoolTemplate != oldMiner.PoolTemplate ||
            newMiner.WalletWorkerTemplate != oldMiner.WalletWorkerTemplate;
    }

    /// <summary>
    /// Отправить команду на инсталляцию майнера агенту.
    /// </summary>
    /// <param name="newMiner"> Новый майнер. </param>
    /// <param name="oldMiner"> Старый майнер. </param>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    private async Task SendInstallCommand(Miner newMiner,
                                          Miner oldMiner,
                                          Guid userId,
                                          CancellationToken cancellationToken)
    {
        var rigIds = await _mediator.Send(new GetRigsIdsByMinerCoincidenceQuery(
            oldMiner.Name,
            oldMiner.Version,
            userId), cancellationToken);

        await _bus.Enqueue(new InstallMinerCommand(
                newMiner.Name,
                newMiner.Version,
                newMiner.InstallationUrl,
                newMiner.PoolTemplate,
                newMiner.WalletWorkerTemplate,
                newMiner.Type.ToMinerTypeContract()),
            rigIds,
            userId,
            cancellationToken: cancellationToken
        );
    }
}