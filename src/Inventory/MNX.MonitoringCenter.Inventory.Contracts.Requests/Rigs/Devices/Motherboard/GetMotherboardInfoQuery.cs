using MediatR;
using MNX.Application.UseCases.Results;

namespace MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Motherboard;

using Motherboard = Contracts.Devices.Motherboard.Motherboard;

/// <summary>
/// Запрос на получение материнской платы.
/// </summary>
/// <param name="UserId"> Идентификатор пользователя. </param>
/// <param name="RigId"> Идентификатор рига. </param>
public sealed record GetMotherboardInfoQuery(Guid UserId, Guid RigId) : IRequest<Result<Motherboard>>;
