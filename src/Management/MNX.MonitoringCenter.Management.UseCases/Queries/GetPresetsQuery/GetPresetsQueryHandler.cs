using AutoMapper;
using MediatR;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;

namespace MNX.MonitoringCenter.Management.UseCases.Queries.GetPresetsQuery;

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

    public async IAsyncEnumerable<PresetModel> Handle(GetPresetsQuery request, CancellationToken cancellationToken)
    {
        await foreach (var preset in _repository.GetAllAvailable(request.GpuName, request.UserId))
        {
            yield return _mapper.Map<PresetModel>(preset);
        }
    }
}