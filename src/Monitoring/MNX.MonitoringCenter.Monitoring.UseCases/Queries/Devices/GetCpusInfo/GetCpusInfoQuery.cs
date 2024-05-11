using MediatR;
using MNX.MonitoringCenter.Monitoring.Core.Devices.Enums;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Queries.Devices.GetCpusInfo;

/// <summary>
/// Запрос на получение информации о процессорах.
/// </summary>
public class GetCpusInfoQuery : IStreamRequest<CpuInfo>
{
    /// <summary>
    /// Спецификация.
    /// </summary>
    public DeviceSpecification Specification { get; }

    public GetCpusInfoQuery(long userId)
    {
        Specification = new DeviceSpecification(userId, new List<MiningDeviceType>()
        {
            MiningDeviceType.CPU,
        });
    }
}
