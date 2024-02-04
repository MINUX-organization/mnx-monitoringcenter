using Kernel.UseCases;
using MediatR;
using MNX.MonitoringCenter.Management.Core.HardwareParameters.Motherboard;

namespace MNX.MonitoringCenter.Management.UseCases.Queries.HardwareParameters.GetMotherboardData;

/// <summary>
/// Запрос на получение информации о материнской плате
/// </summary>
public struct GetMotherboardDataQuery : IRequest<Result<Motherboard>> { }
