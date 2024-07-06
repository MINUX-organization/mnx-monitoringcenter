using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Monitoring.Core.Devices.Gpu;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Queries.Devices.Gpu.GetOverclocking;

/// <summary>
/// Запрос на получение информации о разгоне устройства по идентификатору.
/// </summary>
public class GetGpuOverclockingQuery : IRequest<Result<GpuOverclocking?>>
{
    /// <summary>
    /// Идентификатор видеокарты.
    /// </summary>
    public Guid GpuId { get; }

    public GetGpuOverclockingQuery(Guid gpuId)
    {
        GpuId = gpuId;
    }
}
