using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Inventory.Contracts.Gpu.Restrictions;

namespace MNX.MonitoringCenter.Inventory.Contracts.Queries.Gpu;

/// <summary>
/// Запрос на получение ограничений видеокарты.
/// </summary>
/// <param name="GpuName"> Название видеокарты. </param>
public sealed record GetGpuRestrictionsQuery(string GpuName) : IRequest<Result<GpuRestrictions>>;
