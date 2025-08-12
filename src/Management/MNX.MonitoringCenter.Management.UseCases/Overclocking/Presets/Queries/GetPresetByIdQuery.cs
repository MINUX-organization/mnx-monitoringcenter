using MediatR;
using MNX.Application.UseCases.Requests;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.Contracts.Presets;

namespace MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets.Queries;

/// <summary>
/// Запрос на получение сохранённого пресета по идентификатору.
/// </summary>
/// <param name="PresetId"> Идентификатор пресета. </param>
/// <param name="UserId"> Идентификатор пользователя. </param>
public sealed record GetPresetByIdQuery(Guid PresetId, Guid UserId) : IUserableRequest<Result<PresetModel>>;

/// <summary>
/// Обработчик запроса на получение сохранённого пресета по идентификатору.
/// </summary>
public class GetPresetByIdQueryHandler : IRequestHandler<GetPresetByIdQuery, Result<PresetModel>>
{
    private readonly IPresetRepository _repository;

    private readonly IPresetMapper _presetMapper;

    ///
    public GetPresetByIdQueryHandler(IPresetRepository repository, IPresetMapper presetMapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _presetMapper = presetMapper ?? throw new ArgumentNullException(nameof(presetMapper));
    }

    ///
    public async Task<Result<PresetModel>> Handle(GetPresetByIdQuery request, CancellationToken cancellationToken)
    {
        var preset = await _repository.GetById(request.PresetId, request.UserId, cancellationToken);
        if (preset is null)
        {
            return Result<PresetModel>.Invalid($"Preset with id equaled {request.PresetId} was not found");
        }
        return Result<PresetModel>.Success(_presetMapper.MapToModel(preset));
    }
}