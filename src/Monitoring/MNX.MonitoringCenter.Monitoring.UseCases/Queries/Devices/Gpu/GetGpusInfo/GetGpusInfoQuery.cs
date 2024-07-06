using MediatR;
using MNX.MonitoringCenter.Monitoring.Core.Devices.Enums;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Queries.Devices.Gpu.GetGpusInfo;

/// <summary>
/// Запрос на получение информации о видеокартах.
/// </summary>
public class GetGpusInfoQuery : IStreamRequest<GpuInfo>
{
    /// <summary>
    /// Спецификация.
    /// </summary>
    public DeviceSpecification Specification { get; }

    public GetGpusInfoQuery(long userId)
    {
        Specification = new DeviceSpecification(userId, new List<MiningDeviceType>()
        {
            MiningDeviceType.GPU
        });
    }
}
