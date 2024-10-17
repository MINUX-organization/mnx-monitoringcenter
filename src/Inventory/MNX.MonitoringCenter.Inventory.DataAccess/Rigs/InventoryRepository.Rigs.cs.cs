using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Inventory.Contracts;
using MNX.MonitoringCenter.Inventory.Contracts.Requests;
using MNX.MonitoringCenter.Inventory.DataAccess.Rigs;

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
