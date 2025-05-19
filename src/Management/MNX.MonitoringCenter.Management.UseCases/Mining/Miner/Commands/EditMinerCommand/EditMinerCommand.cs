using MediatR;
using AutoMapper;
using MNX.Application.UseCases.Results;
using MNX.Application.UseCases.Requests;
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
public sealed record EditMinerCommand(MinerInputModel Model, Guid MinerId, Guid UserId)
    : IUserableValidatableCommand<Unit>;

/// <summary>
/// Обработчик команды <see cref="EditMinerCommand"/>.
/// </summary>
public class EditMinerCommandHandler : IRequestHandler<EditMinerCommand, Result<Unit>>
{
    private readonly IMinerRepository _minerRepository;

    public EditMinerCommandHandler(IMinerRepository minerRepository)
    {
        _minerRepository = minerRepository ?? throw new ArgumentNullException(nameof(minerRepository));
    }

    public async Task<Result<Unit>> Handle(EditMinerCommand request, CancellationToken cancellationToken)
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
        return Result<Unit>.Empty();
    }
}