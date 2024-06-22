using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Monitoring.Core;
using MNX.MonitoringCenter.Monitoring.UseCases.Abstractions;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Queries.Devices.GetGpuOverclocking;

/// <summary>
/// Обработчик запроса на получение разгона видеокарты по идентификатору.
/// </summary>
public class GetOverclockingQueryHandler : IRequestHandler<GetOverclockingQuery, Result<Overclocking?>>
{
    /// <summary>
    /// Репозиторий для разгонов видеокарт.
    /// </summary>
    private readonly IOverclockingRepository _overclockingRepository;

    public GetOverclockingQueryHandler(IOverclockingRepository overclockingRepository)
    {
        _overclockingRepository = overclockingRepository ?? throw new ArgumentNullException(nameof(overclockingRepository));
    }

    public async Task<Result<Overclocking?>> Handle(GetOverclockingQuery request, CancellationToken cancellationToken)
    {
        var overclocking = await _overclockingRepository.GetOverclockingById(request.DeviceId);

        return overclocking != null
            ? Result<Overclocking?>.Success(overclocking)
            : Result<Overclocking?>.Invalid("Invalid device Type or Id.");
    }
}
