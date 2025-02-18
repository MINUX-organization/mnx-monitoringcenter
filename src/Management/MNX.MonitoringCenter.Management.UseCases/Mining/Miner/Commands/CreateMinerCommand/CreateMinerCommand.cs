using AutoMapper;
using MediatR;
using MNX.Application.UseCases.Requests;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.Contracts.Miner;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.Models;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.CreateMinerCommand;

using Miner = Core.Mining.Miner.Miner;

public sealed record CreateMinerCommand(MinerInputModel Model, Guid UserId) : IUserableValidatableCommand<MinerModel>;

public class CreateMinerCommandHandler : IRequestHandler<CreateMinerCommand, Result<MinerModel>>
{
    private readonly IMinerRepository _minerRepository;
    private readonly IMapper _mapper;

    public CreateMinerCommandHandler(IMinerRepository minerRepository, IMapper mapper)
    {
        _minerRepository = minerRepository ?? throw new ArgumentNullException(nameof(minerRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<MinerModel>> Handle(CreateMinerCommand request, CancellationToken cancellationToken)
    {
        var miner = _mapper.Map<Miner>(request.Model);
        miner.OwnerId = request.UserId;
        await _minerRepository.Add(miner, cancellationToken);
        return Result<MinerModel>.SuccessfullyCreated(_mapper.Map<MinerModel>(miner));
    }
}