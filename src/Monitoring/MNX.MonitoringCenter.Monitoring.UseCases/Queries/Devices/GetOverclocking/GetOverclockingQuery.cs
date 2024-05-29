using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Monitoring.Core;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Queries.Devices.GetGpuOverclocking;

/// <summary>
/// Запрос на получение информации о разгоне устройства по идентификатору.
/// </summary>
public class GetOverclockingQuery : IRequest<Result<Overclocking?>>
{
    /// <summary>
    /// Идентификатор устройства.
    /// </summary>
    public Guid DeviceId { get; set; }

    public GetOverclockingQuery(Guid deviceId)
    {
        DeviceId = deviceId;
    }
}
