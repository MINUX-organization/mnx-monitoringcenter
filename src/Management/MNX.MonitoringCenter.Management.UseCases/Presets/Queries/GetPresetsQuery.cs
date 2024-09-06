using AutoMapper;
using MediatR;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.Core;
using System.Runtime.CompilerServices;

namespace MNX.MonitoringCenter.Management.UseCases.Presets.Queries;

/// <summary>
/// Запрос на получение сохранённых пресетов для выбранной серии GPU
/// </summary>
public class GetPresetsQuery : IStreamRequest<PresetModel>
{
    /// <summary>
    /// Название GPU
    /// </summary>
    public string? GpuName { get; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; }

    public GetPresetsQuery(string? gpuName, Guid userId)
    {
        GpuName = gpuName;
        UserId = userId;
    }
}

/// <summary>
/// Обработчик запроса на получение сохранённых пресетов для выбранной серии GPU
/// </summary>
public class GetPresetsQueryHandler : IStreamRequestHandler<GetPresetsQuery, PresetModel>
{
    private readonly IPresetRepository _repository;

    private readonly IMapper _mapper;

    public GetPresetsQueryHandler(IPresetRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async IAsyncEnumerable<PresetModel> Handle(GetPresetsQuery request,
                                                     [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (Preset preset in _repository.GetAllAvailable(request.GpuName, request.UserId))
        {
            yield return _mapper.Map<PresetModel>(preset);
        }
    }
}