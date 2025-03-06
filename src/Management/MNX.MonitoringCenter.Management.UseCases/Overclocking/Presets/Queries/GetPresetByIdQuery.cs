using AutoMapper;
using MediatR;
using MNX.Application.UseCases.Requests;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.Contracts.Presets;

namespace MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets.Queries;

/// <summary>
/// Запрос на получение сохранённого пресета по идентификатору.
/// </summary>
public sealed record GetPresetByIdQuery : IUserableRequest<Result<PresetModel>>
{
    /// <summary>
    /// Идентификатор пресета.
    /// </summary>
    public Guid PresetId { get; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; }

    public GetPresetByIdQuery(Guid presetId, Guid userId)
    {
        PresetId = presetId;
        UserId = userId;
    }
}

/// <summary>
/// Обработчик запроса на получение сохранённого пресета по идентификатору.
/// </summary>
public class GetPresetByIdQueryHandler : IRequestHandler<GetPresetByIdQuery, Result<PresetModel>>
{
    private readonly IPresetRepository _repository;

    private readonly IMapper _mapper;

    public GetPresetByIdQueryHandler(IPresetRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<PresetModel>> Handle(GetPresetByIdQuery request, CancellationToken cancellationToken)
    {
        var preset = await _repository.GetAvailableById(request.PresetId, request.UserId, cancellationToken);
        if (preset is null)
        {
            return Result<PresetModel>.Invalid("Preset with this id must exist");
        }
        return Result<PresetModel>.Success(_mapper.Map<PresetModel>(preset));
    }
}