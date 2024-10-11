using System.Runtime.CompilerServices;
using AutoMapper;
using MediatR;
using MNX.MonitoringCenter.Management.UseCases.Miner.Models;

namespace MNX.MonitoringCenter.Management.UseCases.Miner.Queries;

/// <summary>
/// Запрос на получение списка доступных майнеров.
/// </summary>
public sealed record GetAvailableMinersQuery : IStreamRequest<MinerOutputModel>;

/// <summary>
/// Обработчик запроса на получения списка доступных майнеров.
/// </summary>
public class GetAvailableMinersQueryHandler : IStreamRequestHandler<GetAvailableMinersQuery, MinerOutputModel>
{
    private readonly IMinerRepository _repository;
    private readonly IMapper _mapper;

    public GetAvailableMinersQueryHandler(IMinerRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async IAsyncEnumerable<MinerOutputModel> Handle(GetAvailableMinersQuery request, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var miner in _repository.GetAvailableMiners().WithCancellation(cancellationToken))
        {
            yield return _mapper.Map<MinerOutputModel>(miner);
        }
    }
}