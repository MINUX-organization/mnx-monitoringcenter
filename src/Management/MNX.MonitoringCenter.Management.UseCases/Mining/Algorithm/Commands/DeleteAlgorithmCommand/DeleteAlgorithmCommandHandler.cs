using MediatR;
using System.Transactions;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Algorithm.Commands.DeleteAlgorithmCommand;

/// <summary>
/// Обработчик команды <see cref="DeleteAlgorithmCommand"/>.
/// </summary>
public class DeleteAlgorithmCommandHandler : IRequestHandler<DeleteAlgorithmCommand, Result<Unit>>
{
    private readonly IAlgorithmRepository _algorithmRepository;

    private readonly IMinerRepository _minerRepository;

    public DeleteAlgorithmCommandHandler(IAlgorithmRepository algorithmRepository, IMinerRepository minerRepository)
    {
        _algorithmRepository = algorithmRepository 
            ?? throw new ArgumentNullException(nameof(algorithmRepository));
        _minerRepository = minerRepository 
            ?? throw new ArgumentNullException(nameof(minerRepository));
    }

    public async Task<Result<Unit>> Handle(DeleteAlgorithmCommand request, CancellationToken cancellationToken)
    {
        var algorithmId = request.AlgorithmId;
        var userId = request.UserId;

        var algorithm = await _algorithmRepository.GetById(algorithmId, userId);

        if (algorithm!.UserId == null)
            return Result<Unit>.Invalid("Domain algorithms cannot be deleted");

        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            await _minerRepository.RemoveAllMinerAlgorithmsById(algorithmId);
            await _algorithmRepository.RemoveUserAlgorithm(algorithmId, userId);

            transaction.Complete();
        }

        return Result<Unit>.Empty();
    }
}