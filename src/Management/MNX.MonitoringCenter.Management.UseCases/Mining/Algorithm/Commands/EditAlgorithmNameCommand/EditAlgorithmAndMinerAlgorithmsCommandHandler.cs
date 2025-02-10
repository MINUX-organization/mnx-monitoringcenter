using MediatR;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Algorithm.Commands.EditAlgorithmNameCommand;

/// <summary>
/// Обработчик команды <see cref="EditAlgorithmAndMinerAlgorithmsCommand"/>.
/// </summary>
public class EditAlgorithmAndMinerAlgorithmsCommandHandler 
    : IRequestHandler<EditAlgorithmAndMinerAlgorithmsCommand, Result<Unit>>
{
    private readonly IAlgorithmRepository _algorithmRepository;
    private readonly IMinerRepository _minerRepository;

    public EditAlgorithmAndMinerAlgorithmsCommandHandler(IAlgorithmRepository algorithmRepository,
                                                  IMinerRepository minerRepository)
    {
        _algorithmRepository = algorithmRepository
            ?? throw new ArgumentNullException(nameof(algorithmRepository));
        _minerRepository = minerRepository ?? throw new ArgumentNullException(nameof(minerRepository));
    }

    public async Task<Result<Unit>> Handle(EditAlgorithmAndMinerAlgorithmsCommand request,
                                           CancellationToken cancellationToken)
    {
        var model = request.Model;
        var bindings = model.Bindings;
        var relativeNames = bindings.Select(x => x.RelativeName).ToList();
        var minerIds = bindings.Select(x => x.MinerId).ToList();

        var algorithm = await _algorithmRepository.GetById(request.AlgorithmId,
                                                           request.UserId);

        if (algorithm!.UserId == null)
            return Result<Unit>.Invalid("Domain algorithms cannot be edited");

        await _algorithmRepository.EditAlgorithmName(request.AlgorithmId,
                                                     request.UserId,
                                                     model.FullName);

        await _minerRepository.EditMinerBindingsByAlgorithmId(request.AlgorithmId,
                                                              relativeNames,
                                                              minerIds);

        return Result<Unit>.Empty();
    }
}
