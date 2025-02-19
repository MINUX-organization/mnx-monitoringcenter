using MediatR;
using System.Transactions;
using MNX.Application.UseCases.Results;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Algorithm.Commands.EditAlgorithmNameAndMinerAlgorithmsCommand;

/// <summary>
/// Обработчик команды <see cref="EditAlgorithmAndMinerAlgorithmsCommand"/>.
/// </summary>
public class EditAlgorithmAndMinerAlgorithmsCommandHandler
    : IRequestHandler<EditAlgorithmAndMinerAlgorithmsCommand, Result<Unit>>
{
    private readonly IAlgorithmRepository _algorithmRepository;
    private readonly IMinerAlgorithmRepository _minerRepository;

    public EditAlgorithmAndMinerAlgorithmsCommandHandler(IAlgorithmRepository algorithmRepository,
                                                  IMinerAlgorithmRepository minerAlgorithmRepository)
    {
        _algorithmRepository = algorithmRepository
            ?? throw new ArgumentNullException(nameof(algorithmRepository));
        _minerRepository = minerAlgorithmRepository
            ?? throw new ArgumentNullException(nameof(minerAlgorithmRepository));
    }

    public async Task<Result<Unit>> Handle(EditAlgorithmAndMinerAlgorithmsCommand request,
                                           CancellationToken cancellationToken)
    {
        var model = request.Model;
        var bindings = model.Bindings;
        
        if (await _algorithmRepository.Exists(request.AlgorithmId,
                                               model.FullName,
                                               request.UserId,
                                               cancellationToken))
        {
            return Result<Unit>
                .Invalid("Algorithm with that name already exists");
        }

        var algorithm = await _algorithmRepository.GetById(request.AlgorithmId,
                                                           request.UserId);

        if (algorithm is null)
            return Result<Unit>.Invalid(
                $"Algorithm with id equaled {request.AlgorithmId} was not found!");

        if (algorithm.IsDomain())
            return Result<Unit>.Invalid("Domain algorithms cannot be edited");

        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            await _algorithmRepository.EditAlgorithmName(request.AlgorithmId,
                                                         request.UserId,
                                                         model.FullName);

            await _minerRepository.EditMinerBindingsByAlgorithmId(request.AlgorithmId,
                                                                  bindings);

            transaction.Complete();
        }

        return Result<Unit>.Empty();
    }
}