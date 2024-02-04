using MediatR;
using MNX.MonitoringCenter.Management.Core.HardwareParameters;

namespace MNX.MonitoringCenter.Management.UseCases.Queries.HardwareParameters.GetRamsData;

/// <summary>
/// Запрос на получение информации о плашках оперативной памяти
/// </summary>
public struct GetRamsDataQuery : IStreamRequest<Ram> { }
