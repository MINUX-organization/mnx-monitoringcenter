using MediatR;
using System.Transactions;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.Core.Mining.Miner;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Algorithm.Commands.AddAlgorithmCommand;

using Algorithm = Core.Mining.Algorithm;

/// <summary>
/// Обработчик команды <see cref="AddAlgorithmCommand"/>.
/// </summary>
public class AddAlgorithmCommandHandler : IRequestHandler<AddAlgorithmCommand, Result<Algorithm>>
{
    private readonly IMinerRepository _minerRepository;

    private readonly IAlgorithmRepository _algorithmRepository;

    public AddAlgorithmCommandHandler(IMinerRepository minerRepository,
                                         IAlgorithmRepository algorithmRepository)
    {
        _minerRepository = minerRepository
            ?? throw new ArgumentNullException(nameof(minerRepository));
        _algorithmRepository = algorithmRepository
            ?? throw new ArgumentNullException(nameof(algorithmRepository));
    }

    public async Task<Result<Algorithm>> Handle(AddAlgorithmCommand request,
                                                      CancellationToken cancellationToken)
    {
        var model = request.Model;
        var bindings = model.Bindings;

        var algorithm = new Algorithm 
        {
            Name = model.FullName,
            UserId = request.UserId
        };
        
        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            await _algorithmRepository.AddAsync(algorithm);

            foreach (var binding in bindings)
            {
                await _minerRepository.AddMinerAlgorithm(new MinerAlgorithm()
                {
                    Name = binding.RelativeName,
                    AlgorithmId = algorithm.Id,
                    MinerId = binding.MinerId
                });
            }

            transaction.Complete();
        }

        return Result<Algorithm>.SuccessfullyCreated(algorithm);
    }
}
