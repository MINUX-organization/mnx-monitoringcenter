using AutoMapper;
using MediatR;
using MNX.Application.UseCases.Requests;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.Contracts.Presets;

namespace MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets.Queries;

/// <summary>
/// Запрос на получение сохранённого пресета по идентификатору
/// </summary>
public sealed record GetPresetsByIdQuery : IUserableRequest<Result<PresetModel>>
{
    /// <summary>
    /// Идентификатор пресета
    /// </summary>
    public Guid PresetId { get; }

    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid UserId { get; }

    public GetPresetsByIdQuery(Guid presetId, Guid userId)
    {
        PresetId = presetId;
        UserId = userId;
    }
}

/// <summary>
/// Обработчик запроса на получение сохранённого пресета по идентификатору
/// </summary>
public class GetPresetsByIdQueryHandler : IRequestHandler<GetPresetsByIdQuery, Result<PresetModel>>
{
    private readonly IPresetRepository _repository;

    private readonly IMapper _mapper;

    public GetPresetsByIdQueryHandler(IPresetRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<PresetModel>> Handle(GetPresetsByIdQuery request, CancellationToken cancellationToken)
    {
        var preset = _repository.GetAvailableById(request.PresetId, request.UserId, cancellationToken);
        if (preset.Result == null)
        {
            return Result<PresetModel>.Invalid("Preset with this id must exist");
        }
        return Result<PresetModel>.Success(_mapper.Map<PresetModel>(preset.Result));
    }
}