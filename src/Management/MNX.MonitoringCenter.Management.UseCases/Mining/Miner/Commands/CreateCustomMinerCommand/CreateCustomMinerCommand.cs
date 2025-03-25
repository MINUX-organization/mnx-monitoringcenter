using MediatR;
using AutoMapper;
using MNX.Application.UseCases.Results;
using MNX.Application.UseCases.Requests;
using MNX.RigCommander.MessageQueue.Clients.Bus;
using MNX.MonitoringCenter.Management.Contracts.Miner;
using MNX.MonitoringCenter.Management.Agent.Commands.Mining;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.Models;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.CreateCustomMinerCommand;

using Miner = Core.Mining.Miner.Miner;

public sealed record CreateCustomMinerCommand(MinerInputModel Model, Guid UserId)
    : IUserableValidatableCommand<MinerModel>;

public class CreateCustomMinerCommandHandler : IRequestHandler<CreateCustomMinerCommand, Result<MinerModel>>
{
    private readonly IMinerRepository _minerRepository;
    private readonly IMapper _mapper;
    private readonly IQueueBusClient _bus;
    private readonly IRigRepository _rigRepository;

    public CreateCustomMinerCommandHandler(
        IMinerRepository minerRepository, 
        IMapper mapper, 
        IQueueBusClient bus,
        IRigRepository rigRepository)
    {
        _minerRepository = minerRepository ?? throw new ArgumentNullException(nameof(minerRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        _rigRepository = rigRepository ?? throw new ArgumentNullException(nameof(rigRepository));
    }

    public async Task<Result<MinerModel>> Handle(CreateCustomMinerCommand request, CancellationToken cancellationToken)
    {
        var model = request.Model;
        if (await _minerRepository.Exists(request.UserId, model.Name, cancellationToken))
        {
            return Result<MinerModel>.Conflict($"Miner with name {model.Name} already exists");
        }

        var miner = new Miner()
        {
            Name = model.Name,
            Version = model.Version,
            InstallationUrl = model.InstallationUrl,
            SupportedDevices = model.SupportedDevices,
            PoolTemplate = model.PoolTemplate,
            WalletWorkerTemplate = model.WalletWorkerTemplate,
            OwnerId = request.UserId
        };
        
        await _minerRepository.Add(miner, cancellationToken);

        var rigIds = await _rigRepository
            .GetOwnedRigs(request.UserId)
            .ToArrayAsync(cancellationToken);

        await _bus.Enqueue(new InstallCustomMinerCommand(
                miner.Name,
                request.UserId,
                miner.Version,
                miner.InstallationUrl,
                miner.PoolTemplate,
                miner.WalletWorkerTemplate),
            rigIds,
            request.UserId,
            cancellationToken: cancellationToken
        );
        return Result<MinerModel>.SuccessfullyCreated(_mapper.Map<MinerModel>(miner));
    }
}