using AutoMapper;
using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Management.Contracts.Presets;

namespace MNX.MonitoringCenter.Management.UseCases.Presets.Queries;

/// <summary>
/// Запрос на получение сгруппированного по названию видеокарт списка пресетов.
/// </summary>
/// <param name="UserId"> Идентификатор пользователя. </param>
public sealed record GetPresetsGroupedByGpuNameQuery(Guid UserId)
    : IRequest<Result<List<PresetGroup>>>;

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
        var groups = await _presetRepository.GetGroupedList(x => x.GpuName, request.UserId);

        return Result<List<PresetGroup>>.Success(groups.Select(x => new PresetGroup()
        {
            Name = x.Key,
            Presets = _mapper.Map<List<PresetModel>>(x.Value)
        }).ToList());
    }
}
