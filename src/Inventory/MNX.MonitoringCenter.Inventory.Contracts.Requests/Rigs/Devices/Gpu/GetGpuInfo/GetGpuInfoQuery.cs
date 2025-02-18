using MNX.Application.UseCases.Requests;
using MNX.Application.UseCases.Results;

namespace MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Gpu.GetGpuInfo;

/// <summary>
/// Запрос на получение информации о видеокарте.
/// </summary>
/// <param name="GpuId"> Идентификатор видеокарты. </param>
/// <param name="UserId"> Идентификатор пользователя. </param>
public sealed record GetGpuInfoQuery(Guid GpuId, Guid UserId) : IUserableRequest<Result<GpuInfo>>;
