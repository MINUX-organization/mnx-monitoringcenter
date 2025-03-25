using MediatR;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.Contracts.AlgorithmBinding;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Algorithm.Queries.GetAlgorithmById;

/// <summary>
/// Обработчик запроса <see cref="GetAlgorithmByIdQuery"/>.
/// </summary>
public class GetAlgorithmByIdQueryHandler 
    : IRequestHandler<GetAlgorithmByIdQuery, Result<AlgorithmBindingModel>>
{
    private readonly IAlgorithmRepository _algorithmRepository;
    private readonly IMinerAlgorithmRepository _minerAlgorithmRepository;

    public GetAlgorithmByIdQueryHandler(IAlgorithmRepository algorithmRepository,
                                        IMinerAlgorithmRepository minerAlgorithmRepository)
    {
        _algorithmRepository = algorithmRepository 
            ?? throw new ArgumentNullException(nameof(algorithmRepository));
        _minerAlgorithmRepository = minerAlgorithmRepository
            ?? throw new ArgumentNullException(nameof(minerAlgorithmRepository)); ;
    }

    public async Task<Result<AlgorithmBindingModel>> Handle(GetAlgorithmByIdQuery request,
                                                            CancellationToken cancellationToken)
    {
        var algorithm = await _algorithmRepository.GetById(request.AlgorithmId,
                                                           request.UserId,
                                                           cancellationToken);

        if (algorithm == null)
            return Result<AlgorithmBindingModel>.Invalid("Algorithm was not found");

        var bindings = _minerAlgorithmRepository.GetMinerAlgorithmsByAlgorithmId(request.AlgorithmId);

        var bindingModels = new List<RelativeNameBindingModel>();
        await foreach (var binding in bindings.WithCancellation(cancellationToken))
        {
            var newBinding = new RelativeNameBindingModel(binding.Name, binding.MinerId);
            bindingModels.Add(newBinding);
        }

        var result = new AlgorithmBindingModel(algorithm.Name, bindingModels);

        return Result<AlgorithmBindingModel>.Success(result);
    }
}
