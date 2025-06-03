using MediatR;
using System.Transactions;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.Core.Mining.Miner;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Algorithm.Commands.AddAlgorithmCommand;

using Algorithm = Core.Mining.Algorithm;

/// <summary>
/// Обработчик команды <see cref="AddAlgorithmCommand"/>.
/// </summary>
public class AddAlgorithmCommandHandler : IRequestHandler<AddAlgorithmCommand, Result<Algorithm>>
{
    private readonly IMinerAlgorithmRepository _minerRepository;

    private readonly IAlgorithmRepository _algorithmRepository;

    public AddAlgorithmCommandHandler(IMinerAlgorithmRepository minerAlgorithmRepository,
                                      IAlgorithmRepository algorithmRepository)
    {
        _minerRepository = minerAlgorithmRepository
            ?? throw new ArgumentNullException(nameof(minerAlgorithmRepository));
        _algorithmRepository = algorithmRepository
            ?? throw new ArgumentNullException(nameof(algorithmRepository));
    }

    public async Task<Result<Algorithm>> Handle(AddAlgorithmCommand request,
                                                CancellationToken cancellationToken)
    {
        var model = request.Model;
        var bindings = model.Bindings;

        if (await _algorithmRepository.Exists(request.UserId,
                                              model.FullName,
                                              cancellationToken))
        {
            return Result<Algorithm>
                .Conflict($"Algorithm with name {model.FullName} already exists");
        }

        var algorithm = new Algorithm 
        {
            Name = model.FullName,
            OwnerId = request.UserId
        };
        
        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            await _algorithmRepository.AddAsync(algorithm);
            
            await _minerRepository.AddRangeAsync(bindings.Select(binding => new MinerAlgorithm()
            {
                Name = binding.RelativeName,
                AlgorithmId = algorithm.Id,
                MinerId = binding.MinerId
            }).ToList());

            transaction.Complete();
        }

        return Result<Algorithm>.SuccessfullyCreated(algorithm);
    }
}
