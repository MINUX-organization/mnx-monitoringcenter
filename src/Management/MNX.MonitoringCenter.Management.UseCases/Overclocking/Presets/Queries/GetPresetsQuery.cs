using AutoMapper;
using MediatR;
using MNX.MonitoringCenter.Management.Contracts.Presets;
using MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets;
using System.Runtime.CompilerServices;

namespace MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets.Queries;

/// <summary>
/// Запрос на получение сохранённых пресетов для выбранной серии GPU
/// </summary>
public sealed record GetPresetsQuery : IStreamRequest<PresetModel>
{
    /// <summary>
    /// Название GPU
    /// </summary>
    public string? GpuName { get; }

    /// <summary>
    /// Спецификация.
    /// </summary>
    public Specification Specification { get; }

    public GetPresetsQuery(string? gpuName, Guid userId)
    {
        GpuName = gpuName;
        Specification = new Specification(userId);
    }

    public GetPresetsQuery(string? gpuName, Guid userId, string filterString, object[] filterParameters)
    {
        GpuName = gpuName;
        Specification = new Specification(userId, filterString, filterParameters);
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
        var presets = _repository.GetAllAvailable(request.GpuName, request.Specification);

        await foreach (var preset in presets.WithCancellation(cancellationToken))
        {
            yield return _mapper.Map<PresetModel>(preset);
        }
    }
}