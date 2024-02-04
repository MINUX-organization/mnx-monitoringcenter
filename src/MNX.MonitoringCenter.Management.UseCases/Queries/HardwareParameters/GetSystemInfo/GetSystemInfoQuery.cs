using Kernel.UseCases;
using MediatR;
using MNX.MonitoringCenter.Management.Core.HardwareParameters;

namespace MNX.MonitoringCenter.Management.UseCases.Queries.HardwareParameters.GetSystemInfo;

/// <summary>
/// Запрос на получение информации о системе
/// </summary>
public class GetSystemInfoQuery : IRequest<Result<SystemInfo>> { }
