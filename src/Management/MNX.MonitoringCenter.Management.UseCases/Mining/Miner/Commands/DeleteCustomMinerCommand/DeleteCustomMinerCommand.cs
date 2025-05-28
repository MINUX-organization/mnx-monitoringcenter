using MediatR;
using MNX.Application.UseCases.Results;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.DeleteMinerCommand;

/// <summary>
/// Команда удаления пользовательского майнера.
/// </summary>
/// <param name="MinerId"> Идентификатор майнера. </param>
/// <param name="UserId"> Идентификатор пользователя. </param>
public record DeleteCustomMinerCommand(Guid MinerId, Guid UserId) : IRequest<Result<Unit>>;

/// <summary>
/// Обработчик команды <see cref="DeleteCustomMinerCommand"/>.
/// </summary>
public class DeleteMinerCommandHandler : IRequestHandler<DeleteCustomMinerCommand, Result<Unit>>
{
    private readonly IMinerRepository _minerRepository;

    public DeleteMinerCommandHandler(IMinerRepository minerRepository)
    {
        _minerRepository = minerRepository ?? throw new ArgumentNullException(nameof(minerRepository));
    }

    public async Task<Result<Unit>> Handle(DeleteCustomMinerCommand request, CancellationToken cancellationToken)
    {
        var miner = await _minerRepository.GetMinerById(request.MinerId, cancellationToken);

        if (miner is null)
            return Result<Unit>.Empty();

        if (miner.IsDomain())
            return Result<Unit>.Invalid("Domain miners cannot be deleted");

        await _minerRepository.Remove(request.MinerId, request.UserId, cancellationToken);
        return Result<Unit>.Empty();
    }
}