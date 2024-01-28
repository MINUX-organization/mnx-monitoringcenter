using MediatR;
using MINUX.Backend.Worker.Core.HardwareParameters.Gpu;
using MINUX.Backend.Worker.UseCases.Abstractions;

namespace MINUX.Backend.Worker.UseCases.Queries.HardwareParameters.GetGpuData;

/// <summary>
/// Обработчик запроса о получении параметров о видеокартах
/// </summary>
public class GetGpusDataQueryHandler : IStreamRequestHandler<GetGpusDataQuery, Gpu>
{
    private readonly IHardwareParametersRepository _repository;

    public GetGpusDataQueryHandler(IHardwareParametersRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public IAsyncEnumerable<Gpu> Handle(GetGpusDataQuery request, CancellationToken cancellationToken)
    {
        return _repository.GetGpusParameters();
    }
}
