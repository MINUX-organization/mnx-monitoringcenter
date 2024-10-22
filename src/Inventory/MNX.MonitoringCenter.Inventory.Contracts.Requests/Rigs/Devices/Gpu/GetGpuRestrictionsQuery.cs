using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Restrictions;

namespace MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Gpu;

/// <summary>
/// Запрос на получение ограничений видеокарты.
/// </summary>
/// <param name="GpuName"> Название видеокарты. </param>
public sealed record GetGpuRestrictionsQuery(string GpuName) : IRequest<Result<GpuRestrictions>>;
