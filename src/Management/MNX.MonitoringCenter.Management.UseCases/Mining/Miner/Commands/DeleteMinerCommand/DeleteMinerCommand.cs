using MediatR;
using MNX.Application.UseCases.Results;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.DeleteMinerCommand;

public record DeleteMinerCommand(Guid MinerId, Guid UserId) : IRequest<Result<Unit>>;

public class DeleteMinerCommandHandler : IRequestHandler<DeleteMinerCommand, Result<Unit>>
{
    private readonly IMinerRepository _minerRepository;

    public DeleteMinerCommandHandler(IMinerRepository minerRepository)
    {
        _minerRepository = minerRepository ?? throw new ArgumentNullException(nameof(minerRepository));
    }

    public async Task<Result<Unit>> Handle(DeleteMinerCommand request, CancellationToken cancellationToken)
    {
        await _minerRepository.Remove(request.MinerId, request.UserId, cancellationToken);
        return Result<Unit>.Success(Unit.Value);
    }
}