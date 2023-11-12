using MediatR;
using MINUX.Backend.Unit.Core;
using MINUX.Backend.Unit.UseCases.Abstractions;

namespace MINUX.Backend.Unit.UseCases.Queries.GetPresetsQuery;

public class GetPresetsQueryHandler : IStreamRequestHandler<GetPresetsQuery, Preset>
{
    private readonly IMainRepository _repository;

    public GetPresetsQueryHandler(IMainRepository repository)
    {
        _repository = repository;
    }

    public IAsyncEnumerable<Preset> Handle(GetPresetsQuery request, CancellationToken cancellationToken)
    {
        return _repository.Presets.GetAll();
    }
}