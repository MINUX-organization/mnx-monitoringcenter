using MediatR;
using MNX.MonitoringCenter.Management.Core.HardwareParameters.Cpu;

namespace MNX.MonitoringCenter.Management.UseCases.Queries.HardwareParameters.GetCpusData;

/// <summary>
/// Запрос на получение параметров о CPUs
/// </summary>
public struct GetCpusDataQuery : IStreamRequest<Cpu> { }
