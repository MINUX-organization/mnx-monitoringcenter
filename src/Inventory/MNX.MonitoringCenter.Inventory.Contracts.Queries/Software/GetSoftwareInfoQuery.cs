using MediatR;
using MNX.Application.UseCases;

namespace MNX.MonitoringCenter.Inventory.Contracts.Queries.Software;

/// <summary>
/// Запрос на получение информации о программном обеспечении.
/// </summary>
/// <param name="UserId"> Идентификатор пользователя. </param>
/// <param name="RigId"> Идентификатор рига. </param>
public sealed record GetSoftwareInfoQuery(Guid UserId, Guid RigId) : IRequest<Result<SoftwareInventory>>;
