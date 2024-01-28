using MediatR;
using MINUX.Backend.Worker.Core.HardwareParameters.Cpu;
using MINUX.Backend.Worker.UseCases.Abstractions;

namespace MINUX.Backend.Worker.UseCases.Queries.HardwareParameters.GetCpusData;

/// <summary>
/// Обработчик запроса на получение параметров о CPUs
/// </summary>
public class GetCpusQueryDataHandler : IStreamRequestHandler<GetCpusDataQuery, Cpu>
{
    private readonly IHardwareParametersRepository _repository;

    public GetCpusQueryDataHandler(IHardwareParametersRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public IAsyncEnumerable<Cpu> Handle(GetCpusDataQuery request, CancellationToken cancellationToken)
    {
        return _repository.GetCpusParameters();
    }
}
