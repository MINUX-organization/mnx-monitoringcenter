using MediatR;
using MINUX.Backend.Worker.Core;
using MINUX.Backend.Worker.UseCases.Abstractions;

namespace MINUX.Backend.Worker.UseCases.Queries.GetPresetsQuery;

/// <summary>
/// Обработчик запроса на получение сохранённых пресетов для выбранной серии GPU
/// </summary>
public class GetPresetsQueryHandler : IStreamRequestHandler<GetPresetsQuery, Preset>
{
    private readonly IPresetRepository _repository;

    public GetPresetsQueryHandler(IPresetRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public IAsyncEnumerable<Preset> Handle(GetPresetsQuery request, CancellationToken cancellationToken)
    {
        return _repository.GetPresets(request.GpuName);
    }
}