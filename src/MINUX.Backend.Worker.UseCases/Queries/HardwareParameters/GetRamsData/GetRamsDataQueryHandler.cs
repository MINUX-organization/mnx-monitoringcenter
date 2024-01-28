using MediatR;
using MINUX.Backend.Worker.Core.HardwareParameters;
using MINUX.Backend.Worker.UseCases.Abstractions;

namespace MINUX.Backend.Worker.UseCases.Queries.HardwareParameters.GetRamsData;

/// <summary>
/// Обработчик запроса на получение информации о плашках оперативной памяти
/// </summary>
public class GetRamsDataQueryHandler : IStreamRequestHandler<GetRamsDataQuery, Ram>
{
    private readonly IHardwareParametersRepository _repository;

    public GetRamsDataQueryHandler(IHardwareParametersRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public IAsyncEnumerable<Ram> Handle(GetRamsDataQuery request, CancellationToken cancellationToken)
    {
        return _repository.GetRamsParameters();
    }
}
