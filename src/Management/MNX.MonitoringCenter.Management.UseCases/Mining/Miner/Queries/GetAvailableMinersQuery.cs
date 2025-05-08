using MediatR;
using AutoMapper;
using System.Runtime.CompilerServices;
using MNX.MonitoringCenter.Management.Contracts.Miner;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Queries;

/// <summary>
/// Запрос на получение списка доступных майнеров.
/// </summary>
public sealed record GetAvailableMinersQuery : IStreamRequest<MinerModel>
{
    /// <summary>
    /// Спецификация.
    /// </summary>
    public Specification Specification { get; }

    public GetAvailableMinersQuery(Guid userId)
    {
        Specification = new Specification(userId);
    }

    public GetAvailableMinersQuery(Guid userId,
                                   string filterString,
                                   object[] filterParameters)
    {
        Specification = new Specification(userId, filterString, filterParameters);
    }
}

/// <summary>
/// Обработчик запроса на получения списка доступных майнеров.
/// </summary>
public class GetAvailableMinersQueryHandler : IStreamRequestHandler<GetAvailableMinersQuery, MinerModel>
{
    private readonly IMinerRepository _repository;
    private readonly IMapper _mapper;

    public GetAvailableMinersQueryHandler(IMinerRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async IAsyncEnumerable<MinerModel> Handle(GetAvailableMinersQuery request,
                                                     [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var miners = _repository.GetAvailableMiners(request.Specification);
        await foreach (var miner in miners.WithCancellation(cancellationToken))
        {
            yield return _mapper.Map<MinerModel>(miner);
        }
    }
}