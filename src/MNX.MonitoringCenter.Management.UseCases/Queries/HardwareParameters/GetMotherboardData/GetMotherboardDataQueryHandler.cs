using Kernel.UseCases;
using MediatR;
using MNX.MonitoringCenter.Management.Core.HardwareParameters.Motherboard;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;

namespace MNX.MonitoringCenter.Management.UseCases.Queries.HardwareParameters.GetMotherboardData;

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
