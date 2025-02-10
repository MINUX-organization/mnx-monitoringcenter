using MediatR;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner;
using MNX.MonitoringCenter.Management.Contracts.AlgorithmBinding;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Algorithm.Queries.GetAlgorithmById;

/// <summary>
/// Обработчик запроса <see cref="GetAlgorithmByIdQuery"/>.
/// </summary>
public class GetAlgorithmByIdQueryHandler 
    : IRequestHandler<GetAlgorithmByIdQuery, Result<AlgorithmBindingModel>>
{
    private readonly IAlgorithmRepository _algorithmRepository;
    private readonly IMinerRepository _minerRepository;

    public GetAlgorithmByIdQueryHandler(IAlgorithmRepository algorithmRepository,
                                        IMinerRepository minerRepository)
    {
        _algorithmRepository = algorithmRepository 
            ?? throw new ArgumentNullException(nameof(algorithmRepository));
        _minerRepository = minerRepository
            ?? throw new ArgumentNullException(nameof(minerRepository)); ;
    }

    public async Task<Result<AlgorithmBindingModel>> Handle(GetAlgorithmByIdQuery request,
                                                            CancellationToken cancellationToken)
    {
        var algorithm = await _algorithmRepository.GetById(request.AlgorithmId, request.UserId);

        if (algorithm == null)
            return Result<AlgorithmBindingModel>.Invalid("Algorithm was not found");

        var bindings = _minerRepository.GetMinerAlgorithmsByAlgorithmId(request.AlgorithmId);

        var bindingModels = new List<RelativeNameBindingModel>();
        await foreach (var binding in bindings)
        {
            var newBinding = new RelativeNameBindingModel(binding.Name, binding.MinerId);
            bindingModels.Add(newBinding);
        }

        var result = new AlgorithmBindingModel(algorithm.Name, bindingModels);

        return Result<AlgorithmBindingModel>.Success(result);
    }
}
