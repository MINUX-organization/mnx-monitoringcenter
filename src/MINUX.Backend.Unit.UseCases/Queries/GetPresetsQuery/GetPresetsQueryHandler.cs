using MediatR;
using MINUX.Backend.Unit.Core;
using MINUX.Backend.Unit.UseCases.Abstractions;

namespace MINUX.Backend.Unit.UseCases.Queries.GetPresetsQuery;

public class GetPresetsQueryHandler : IStreamRequestHandler<GetPresetsQuery, Preset>
{
    private readonly IPresetRepository _repository;

    public GetPresetsQueryHandler(IPresetRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public IAsyncEnumerable<Preset> Handle(GetPresetsQuery request, CancellationToken cancellationToken)
    {
        return _repository.GetAll();
    }
}