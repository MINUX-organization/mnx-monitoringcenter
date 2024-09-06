using MediatR;
using MNX.MonitoringCenter.Monitoring.Core.Devices.Enums;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Queries.Devices.Cpu.GetCpusInfo;

/// <summary>
/// Запрос на получение информации о процессорах.
/// </summary>
public class GetCpusInfoQuery : IStreamRequest<CpuInfo>
{
    /// <summary>
    /// Спецификация.
    /// </summary>
    public DeviceSpecification Specification { get; }

    public GetCpusInfoQuery(Guid userId)
    {
        Specification = new DeviceSpecification(userId, new List<MiningDeviceType>()
        {
            MiningDeviceType.CPU,
        });
    }
}
