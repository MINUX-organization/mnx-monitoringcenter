using Kernel.UseCases;
using MediatR;
using MINUX.Backend.Worker.Core.HardwareParameters.Motherboard;
using MINUX.Backend.Worker.UseCases.Abstractions;

namespace MINUX.Backend.Worker.UseCases.Queries.HardwareParameters.GetMotherboardData;

/// <summary>
/// Обработчик запроса на получение информации о материнской плате
/// </summary>
public class GetMotherboardDataQueryHandler : IRequestHandler<GetMotherboardDataQuery, Result<Motherboard>>
{
    private readonly IHardwareParametersRepository _repository;

    public GetMotherboardDataQueryHandler(IHardwareParametersRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Result<Motherboard>> Handle(GetMotherboardDataQuery request, CancellationToken cancellationToken)
    {
        return Result<Motherboard>.Success(await _repository.GetMotherboardParameters());
    }
}
