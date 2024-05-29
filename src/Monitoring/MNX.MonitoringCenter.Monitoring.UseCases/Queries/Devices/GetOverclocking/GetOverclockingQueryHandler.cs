using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Monitoring.Core;
using MNX.MonitoringCenter.Monitoring.UseCases.Abstractions;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Queries.Devices.GetGpuOverclocking;

/// <summary>
/// Обработчик запроса на получение разгона устройства по идентификатору.
/// </summary>
public class GetOverclockingQueryHandler : IRequestHandler<GetOverclockingQuery, Result<Overclocking?>>
{
    /// <summary>
    /// Репозиторий для майнинг устройств.
    /// </summary>
    private readonly IMiningDeviceRepository _deviceRepository;

    public GetOverclockingQueryHandler(IMiningDeviceRepository deviceRepository)
    {
        _deviceRepository = deviceRepository ?? throw new ArgumentNullException(nameof(deviceRepository));
    }

    public async Task<Result<Overclocking?>> Handle(GetOverclockingQuery request, CancellationToken cancellationToken)
    {
        var overclocking = await _deviceRepository.GetOverclockingById(request.DeviceId);

        if (overclocking == null)
        {
            return Result<Overclocking?>.Invalid("Invalid device Id");
        }

        return Result<Overclocking?>.Success(overclocking);
    }
}
