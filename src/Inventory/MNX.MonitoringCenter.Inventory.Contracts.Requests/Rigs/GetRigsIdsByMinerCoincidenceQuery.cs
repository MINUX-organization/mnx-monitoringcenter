using MediatR;

namespace MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs;

/// <summary>
/// Команда получения списка идентификаторов ригов пользователя
/// по совпадению с установленными майнерами.
/// </summary>
/// <param name="MinerName"> Наименование майнера. </param>
/// <param name="MinerVersion"> Версия майнера. </param>
/// <param name="UserId"> Идентификатор пользователя. </param>
public sealed record GetRigsIdsByMinerCoincidenceQuery(string MinerName,
                                                       string MinerVersion,
                                                       Guid UserId)
    : IRequest<Guid[]>;