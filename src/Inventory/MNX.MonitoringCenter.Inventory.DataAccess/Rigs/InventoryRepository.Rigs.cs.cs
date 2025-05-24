using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Inventory.Contracts;
using MNX.MonitoringCenter.Inventory.DataAccess.Rigs;
using MNX.MonitoringCenter.Inventory.Contracts.Requests;

namespace MNX.MonitoringCenter.Inventory.DataAccess;

/// <summary>
/// Репозиторий инвентаризации ригов.
/// </summary>
public partial class InventoryRepository
{
    /// <summary>
    /// Получить риги по спецификации.
    /// </summary>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Запрашиваемые риги. </returns>
    internal IQueryable<RigDto> GetRigsBySpecification(InventorySpecification specification)
    {
        return _context.Rigs.AsNoTrackingWithIdentityResolution()
                            .Available(specification)
                            .Filter(specification);
    }

    /// <summary>
    /// Получить массив идентификаторов ригов с совпадающими майнерами.
    /// </summary>
    /// <param name="miner"> Связка наименования майнера и его версии. </param>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Массив идентификаторов ригов. </returns>
    internal Task<Guid[]> GetMatchingRigIdsQuery(KeyValuePair<string, string> miner,
                                                 Guid userId,
                                                 CancellationToken cancellationToken)
    {
        return _context.Rigs
            .Where(rig => rig.OwnerId == userId &&
                rig.Inventories.Any(inventory =>
                    inventory.IsCurrent &&
                    inventory.Software.Miners.Any(minerDto =>
                        minerDto.Name == miner.Key &&
                        minerDto.Version == miner.Value)))
            .Select(rig => rig.Id)
            .ToArrayAsync(cancellationToken);
    }

    /// <summary>
    /// Получить массив идентификаторов ригов, на которых не установлен майнер.
    /// </summary>
    /// <param name="rigIds"> Идентификаторы ригов</param>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="miner"> Пара: ключ-значение наименования и версии майнера. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Массив идентификаторов ригов, на которых не установлен майнер. </returns>
    internal Task<Guid[]> GetRigIdsWithoutMiner(Guid[] rigIds,
                                                Guid userId,
                                                KeyValuePair<string, string> miner,
                                                CancellationToken cancellationToken)
    {
        return _context.Rigs
            .Where(rig => rigIds.Contains(rig.Id) && rig.OwnerId == userId)
            .Where(rig => !rig.Inventories
                .Where(inventory => inventory.IsCurrent)
                .SelectMany(inventory => inventory.Software.Miners)
                .Any(minerDto => minerDto.Name == miner.Key && minerDto.Version == miner.Value))
            .Select(rig => rig.Id)
            .ToArrayAsync(cancellationToken);
    }

    /// <summary>
    /// Получить признак существования рига.
    /// </summary>
    /// <param name="rigId"> Идентификатор рига. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns>
    /// <see cref="true"/>, если риг существует, иначе <see cref="false"/>.
    /// </returns>
    internal Task<bool> Exists(Guid rigId, CancellationToken cancellationToken)
    {
        return _context.Rigs.AnyAsync(rig => rig.Id == rigId, cancellationToken);
    }

    /// <summary>
    /// Добавить риг.
    /// </summary>
    /// <param name="rig"> Риг. </param>
    internal async Task AddRig(Rig rig, CancellationToken cancellationToken)
    {
        await _context.Rigs.AddAsync(new RigDto()
        {
            Id = rig.Id,
            OwnerId = rig.OwnerId,
            Name = rig.Name
        },
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
