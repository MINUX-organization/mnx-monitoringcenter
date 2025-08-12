using MediatR;
using MNX.MonitoringCenter.Management.Contracts.Presets;
using System.Runtime.CompilerServices;

namespace MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets.Queries;

/// <summary>
/// Запрос на получение сгруппированного по названию видеокарт списка пресетов.
/// </summary>
public sealed record GetPresetsGroupedByGpuNameQuery : IStreamRequest<PresetGroup>
{
    /// <summary>
    /// Спецификация.
    /// </summary>
    public Specification Specification { get; }

    ///
    public GetPresetsGroupedByGpuNameQuery(Guid userId)
    {
        Specification = new Specification(userId);
    }

    ///
    public GetPresetsGroupedByGpuNameQuery(Guid userId, string filterString, object[] filterParameters)
    {
        Specification = new Specification(userId, filterString, filterParameters);
    }
}

/// <summary>
/// Обработчик <see cref="GetPresetsGroupedByGpuNameQuery"/>.
/// </summary>
public class GetPresetsGroupedByGpuNameQueryHandler
    : IStreamRequestHandler<GetPresetsGroupedByGpuNameQuery, PresetGroup>
{
    private readonly IPresetMapper _presetMapper;

    private readonly IPresetRepository _presetRepository;

    ///
    public GetPresetsGroupedByGpuNameQueryHandler(IPresetMapper presetMapper, IPresetRepository presetRepository)
    {
        _presetMapper = presetMapper ?? throw new ArgumentNullException(nameof(presetMapper));
        _presetRepository = presetRepository ?? throw new ArgumentNullException(nameof(presetRepository));
    }

    ///
    public async IAsyncEnumerable<PresetGroup> Handle(GetPresetsGroupedByGpuNameQuery request,
                                                     [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var groups = _presetRepository.GetGroupedList(x => x.DeviceName, request.Specification);

        await foreach (var group in groups.WithCancellation(cancellationToken))
        {
            yield return new PresetGroup()
            {
                Name = group.Key,
                Presets = _presetMapper.MapToCoreEntitiesList(group.ToList())
            };
        }
    }
}
