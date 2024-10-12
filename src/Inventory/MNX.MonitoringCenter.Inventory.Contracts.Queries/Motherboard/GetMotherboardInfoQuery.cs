using MediatR;
using MNX.Application.UseCases;

namespace MNX.MonitoringCenter.Inventory.Contracts.Queries.Motherboard;

using Motherboard = Contracts.Motherboard.Motherboard;

/// <summary>
/// Запрос на получение материнской платы.
/// </summary>
/// <param name="UserId"> Идентификатор пользователя. </param>
/// <param name="RigId"> Идентификатор рига. </param>
public sealed record GetMotherboardInfoQuery(Guid UserId, Guid RigId) : IRequest<Result<Motherboard>>;
