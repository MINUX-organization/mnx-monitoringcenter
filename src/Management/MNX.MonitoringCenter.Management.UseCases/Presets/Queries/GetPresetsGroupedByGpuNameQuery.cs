using AutoMapper;
using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Management.Contracts.Presets;

namespace MNX.MonitoringCenter.Management.UseCases.Presets.Queries;

/// <summary>
/// Запрос на получение сгруппированного по названию видеокарт списка пресетов.
/// </summary>
public sealed record GetPresetsGroupedByGpuNameQuery : IRequest<Result<List<PresetGroup>>>
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
    : IRequestHandler<GetPresetsGroupedByGpuNameQuery, Result<List<PresetGroup>>>
{
    private readonly IMapper _mapper;

    private readonly IPresetRepository _presetRepository;

    public GetPresetsGroupedByGpuNameQueryHandler(IMapper mapper, IPresetRepository presetRepository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _presetRepository = presetRepository ?? throw new ArgumentNullException(nameof(presetRepository));
    }

    public async Task<Result<List<PresetGroup>>> Handle(GetPresetsGroupedByGpuNameQuery request,
                                                        CancellationToken cancellationToken)
    {
        var groups = await _presetRepository.GetGroupedList(x => x.GpuName, request.Specification);

        return Result<List<PresetGroup>>.Success(groups.Select(x => new PresetGroup
        {
            Name = x.Key,
            Presets = _mapper.Map<List<PresetModel>>(x.Value)
        }).ToList());
    }
}
