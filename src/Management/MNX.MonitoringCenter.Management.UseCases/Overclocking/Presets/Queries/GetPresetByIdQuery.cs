using MediatR;
using AutoMapper;
using MNX.Application.UseCases.Results;
using MNX.Application.UseCases.Requests;
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
            return Result<PresetModel>.Invalid($"Preset with id equaled {request.PresetId} was not found");
        }
        return Result<PresetModel>.Success(_mapper.Map<PresetModel>(preset));
    }
}