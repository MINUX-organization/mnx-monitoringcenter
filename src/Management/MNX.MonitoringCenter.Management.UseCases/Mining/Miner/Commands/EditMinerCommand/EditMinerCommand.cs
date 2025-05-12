using MediatR;
using AutoMapper;
using MNX.Application.UseCases.Results;
using MNX.Application.UseCases.Requests;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.Models;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.EditMinerCommand;

using Miner = Core.Mining.Miner.Miner;

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
    private readonly IMapper _mapper;

    public EditMinerCommandHandler(IMinerRepository minerRepository, IMapper mapper)
    {
        _minerRepository = minerRepository ?? throw new ArgumentNullException(nameof(minerRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<Unit>> Handle(EditMinerCommand request, CancellationToken cancellationToken)
    {
        var model = request.Model;

        if (!await _minerRepository.Exists(request.MinerId, cancellationToken))
        {
            return Result<Unit>.Invalid($"Miner with id equaled {request.MinerId} was not found");
        }
        
        if (await _minerRepository.Exists(request.MinerId, request.UserId, model.Name, cancellationToken))
        {
            return Result<Unit>.Conflict($"Miner with name {model.Name} already exists");
        }

        var newMiner = new Miner()
        {
            Id = request.MinerId,
            OwnerId = request.UserId,
            Name = model.Name,
            Version = model.Version,
            InstallationUrl = model.InstallationUrl,
            SupportedDevices = model.SupportedDevices,
            PoolTemplate = model.PoolTemplate,
            WalletWorkerTemplate = model.WalletWorkerTemplate,
        };
        await _minerRepository.Edit(newMiner, cancellationToken);
        return Result<Unit>.Empty();
    }
}