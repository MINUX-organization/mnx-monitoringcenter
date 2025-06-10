using AutoMapper;
using MediatR;
using MNX.Application.UseCases.Requests;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.Contracts.Miner;
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Enums;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.Models;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.CreateCustomMinerCommand;

using Miner = Core.Mining.Miner.Miner;

/// <summary>
/// Команда создания пользовательского майнера.
/// </summary>
/// <param name="Model"> Модель ввода данных кастомного майнера. </param>
/// <param name="UserId"> Идентификатор пользователя. </param>
public sealed record CreateCustomMinerCommand(MinerInputModel Model, Guid UserId)
    : IUserableValidatableCommand<MinerModel>;

/// <summary>
/// Обработчик команды <see cref="CreateCustomMinerCommand"/>.
/// </summary>
public class CreateCustomMinerCommandHandler : IRequestHandler<CreateCustomMinerCommand, Result<MinerModel>>
{
    private readonly IMinerRepository _minerRepository;
    private readonly IMapper _mapper;

    public CreateCustomMinerCommandHandler(IMinerRepository minerRepository, 
                                           IMapper mapper)
    {
        _minerRepository = minerRepository ?? throw new ArgumentNullException(nameof(minerRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<MinerModel>> Handle(CreateCustomMinerCommand request,
                                                 CancellationToken cancellationToken)
    {
        var model = request.Model;
        if (await _minerRepository.Exists(request.UserId,
                                          model.Name,
                                          model.Version,
                                          cancellationToken))
        {
            return Result<MinerModel>.Conflict(
                $"Miner with name {model.Name} and version {model.Version} already exists");
        }

        var miner = new Miner()
        {
            Name = model.Name,
            Version = model.Version,
            Type = MinerTypeEnum.Custom,
            InstallationUrl = model.InstallationUrl,
            SupportedDevices = model.SupportedDevices,
            PoolTemplate = model.PoolTemplate,
            WalletWorkerTemplate = model.WalletWorkerTemplate,
            MiningMode = model.MiningMode,
            OwnerId = request.UserId
        };
        
        await _minerRepository.Add(miner, cancellationToken);

        return Result<MinerModel>.SuccessfullyCreated(_mapper.Map<MinerModel>(miner));
    }
}