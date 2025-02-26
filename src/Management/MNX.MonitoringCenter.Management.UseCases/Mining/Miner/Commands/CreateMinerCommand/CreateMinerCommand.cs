using AutoMapper;
using MediatR;
using MNX.Application.UseCases.Requests;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.Agent.Commands.Mining;
using MNX.MonitoringCenter.Management.Contracts.Miner;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.Models;
using MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice;
using MNX.RigCommander.MessageQueue.Clients.Bus;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.CreateMinerCommand;

using Miner = Core.Mining.Miner.Miner;

public sealed record CreateMinerCommand(MinerInputModel Model, Guid UserId) : IUserableValidatableCommand<MinerModel>;

public class CreateMinerCommandHandler : IRequestHandler<CreateMinerCommand, Result<MinerModel>>
{
    private readonly IMinerRepository _minerRepository;
    private readonly IMapper _mapper;
    private readonly IQueueBusClient _bus;
    private readonly IMiningDeviceRepository _miningDeviceRepository;

    public CreateMinerCommandHandler(IMinerRepository minerRepository, IMapper mapper, IQueueBusClient bus,
        IMiningDeviceRepository miningDeviceRepository)
    {
        _minerRepository = minerRepository ?? throw new ArgumentNullException(nameof(minerRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        _miningDeviceRepository =
            miningDeviceRepository ?? throw new ArgumentNullException(nameof(miningDeviceRepository));
    }

    public async Task<Result<MinerModel>> Handle(CreateMinerCommand request, CancellationToken cancellationToken)
    {
        var miner = _mapper.Map<Miner>(request.Model);
        miner.OwnerId = request.UserId;
        await _minerRepository.Add(miner, cancellationToken);

        var rigIds = await _miningDeviceRepository.GetAvailable(new Specification(request.UserId))
            .Select(device => device.RigId).ToArrayAsync(cancellationToken: cancellationToken);

        await _bus.Enqueue(
            new InstallCustomMinerCommand(
                miner.Id,
                miner.Version,
                miner.InstallationUrl!,
                miner.PoolTemplate!,
                miner.WalletWorkerTemplate!),
            rigIds,
            request.UserId,
            cancellationToken: cancellationToken
        );
        return Result<MinerModel>.SuccessfullyCreated(_mapper.Map<MinerModel>(miner));
    }
}