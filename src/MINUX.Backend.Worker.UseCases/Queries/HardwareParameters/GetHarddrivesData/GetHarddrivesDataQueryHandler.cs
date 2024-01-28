using MediatR;
using MINUX.Backend.Worker.Core.HardwareParameters.Harddrive;
using MINUX.Backend.Worker.UseCases.Abstractions;

namespace MINUX.Backend.Worker.UseCases.Queries.HardwareParameters.GetHarddriveData;

/// <summary>
/// Обработчик получения информации о жёстких дисках
/// </summary>
public class GetHarddrivesDataQueryHandler : IStreamRequestHandler<GetHarddrivesDataQuery, Harddrive>
{
    private readonly IHardwareParametersRepository _repository;

    public GetHarddrivesDataQueryHandler(IHardwareParametersRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public IAsyncEnumerable<Harddrive> Handle(GetHarddrivesDataQuery request, CancellationToken cancellationToken)
    {
        return _repository.GetHarddrivesParameters();
    }
}
