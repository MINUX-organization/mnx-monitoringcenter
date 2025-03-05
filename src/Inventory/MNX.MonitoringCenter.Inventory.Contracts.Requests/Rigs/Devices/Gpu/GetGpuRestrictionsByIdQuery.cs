using MediatR;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Restrictions;

namespace MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Gpu;

/// <summary>
/// Запрос на получение ограничений по идентификатору видеокарты.
/// </summary>
/// <param name="GpuId"> Идентификатор видеокарты. </param>
public sealed record GetGpuRestrictionsByIdQuery(Guid GpuId) : IRequest<Result<GpuRestrictions>>;