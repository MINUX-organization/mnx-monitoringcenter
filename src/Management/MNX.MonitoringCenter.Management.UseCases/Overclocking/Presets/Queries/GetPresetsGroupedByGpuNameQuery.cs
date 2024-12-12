using AutoMapper;
using MediatR;
using MNX.MonitoringCenter.Management.Contracts.Presets;
using MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets;
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

    public GetPresetsGroupedByGpuNameQuery(Guid userId)
    {
        Specification = new Specification(userId);
    }

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
    private readonly IMapper _mapper;

    private readonly IPresetRepository _presetRepository;

    public GetPresetsGroupedByGpuNameQueryHandler(IMapper mapper, IPresetRepository presetRepository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _presetRepository = presetRepository ?? throw new ArgumentNullException(nameof(presetRepository));
    }

    public async IAsyncEnumerable<PresetGroup> Handle(GetPresetsGroupedByGpuNameQuery request,
                                                     [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var groups = _presetRepository.GetGroupedList(x => x.DeviceName, request.Specification);

        await foreach (var group in groups.WithCancellation(cancellationToken))
        {
            yield return new PresetGroup()
            {
                Name = group.Key,
                Presets = _mapper.Map<List<PresetModel>>(group.ToList())
            };
        }
    }
}
