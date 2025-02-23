using MediatR;
using MNX.Application.UseCases.CommandValidation;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.Contracts.MinerContracts;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Miner;

public record TestMinerCommand(MinerInputModel Model)
    : IValidatableCommand<Unit>;

public class TestMinerCommandHandler : IRequestHandler<TestMinerCommand, Result<Unit>>
{
    private readonly IMinerRepository _repository;

    public TestMinerCommandHandler(IMinerRepository repository)
    {
        _repository = repository ?? throw new NotImplementedException(nameof(repository));
    }

    public async Task<Result<Unit>> Handle(TestMinerCommand request, CancellationToken cancellationToken)
    {
        var model = request.Model;

        var miner = new Core.Mining.Miner.Miner()
        {
            Name = model.Name,
            Version = model.Version,
            SupportedAlgorithms = model.SupportedAlgorithms,
            SupportedDevices = model.SupportedDevices,
            MiningMode = model.MiningMode
        };

        return Result<Unit>.Empty();
    }
}