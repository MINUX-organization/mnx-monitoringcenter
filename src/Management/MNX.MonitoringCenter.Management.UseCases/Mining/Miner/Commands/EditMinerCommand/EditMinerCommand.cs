using AutoMapper;
using MediatR;
using MNX.Application.UseCases.Requests;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.Models;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.EditMinerCommand;

using Miner = Core.Mining.Miner.Miner;

public sealed record EditMinerCommand(MinerInputModel Model, Guid MinerId, Guid UserId)
    : IUserableValidatableCommand<Unit>;

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
        var miner = _mapper.Map<Miner>(request.Model);
        miner.Id = request.MinerId;
        miner.OwnerId = request.UserId;
        await _minerRepository.Edit(miner, cancellationToken);
        return Result<Unit>.Success(Unit.Value);
    }
}