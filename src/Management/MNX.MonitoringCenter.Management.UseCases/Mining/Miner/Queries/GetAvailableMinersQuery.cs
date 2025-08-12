using MediatR;
using MNX.MonitoringCenter.Management.Contracts.Miner;
using System.Runtime.CompilerServices;

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

    ///
    public GetAvailableMinersQuery(Guid userId)
    {
        Specification = new Specification(userId);
    }

    ///
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
    private readonly IMinerMapper _minerMapper;

    ///
    public GetAvailableMinersQueryHandler(IMinerRepository repository, IMinerMapper minerMapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _minerMapper = minerMapper ?? throw new ArgumentNullException(nameof(minerMapper));
    }

    ///
    public async IAsyncEnumerable<MinerModel> Handle(GetAvailableMinersQuery request,
                                                     [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var miners = _repository.GetAvailableMiners(request.Specification);
        await foreach (var miner in miners.WithCancellation(cancellationToken))
        {
            yield return _minerMapper.MapToModel(miner);
        }
    }
}