using MediatR;

namespace MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs;

/// <summary>
/// Запрос на получение идентификаторов рига, у которых не установлен майнер.
/// </summary>
/// <param name="RigIds"> Массив идентификаторов ригов для проверки. </param>
/// <param name="UserId"> Идентфикатор пользователя. </param>
/// <param name="MinerName"> Наименование майнера. </param>
/// <param name="MinerVersion"> Версия майнера. </param>
public sealed record GetRigIdsWithoutMinerQuery(Guid[] RigIds,
                                                Guid UserId,
                                                string MinerName,
                                                string MinerVersion)
    : IRequest<Guid[]>;