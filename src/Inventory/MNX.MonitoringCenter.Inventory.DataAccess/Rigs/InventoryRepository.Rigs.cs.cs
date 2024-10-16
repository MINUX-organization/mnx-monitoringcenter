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
    private readonly Context _context;

    public InventoryRepository(Context context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

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
