using MediatR;
using MNX.MonitoringCenter.Management.Core.HardwareParameters.Gpu;

namespace MNX.MonitoringCenter.Management.UseCases.Queries.HardwareParameters.GetGpuData;

/// <summary>
/// Запрос на получение праметров о видеокартах
/// </summary>
public struct GetGpusDataQuery : IStreamRequest<Gpu> { }
