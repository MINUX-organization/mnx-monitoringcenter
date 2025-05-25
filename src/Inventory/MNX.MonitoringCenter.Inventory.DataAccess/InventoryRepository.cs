using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Inventory.Contracts;
using MNX.MonitoringCenter.Inventory.Contracts.Requests;

namespace MNX.MonitoringCenter.Inventory.DataAccess;

/// <summary>
/// Репозиторий инвентаризации.
/// </summary>
public partial class InventoryRepository
{
    private readonly Context _context;

    private readonly IMapper _mapper;

    public InventoryRepository(Context context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Сохранить инвентаризацию.
    /// </summary>
    /// <param name="rigId"> Идентификатор рига. </param>
    /// <param name="createdDate"> Дата и время создания инвентаризации. </param>
    /// <param name="inventory"> Результат инвентаризации. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    internal async Task Save(Guid rigId, DateTimeOffset createdDate,
                             RigInventoryModel inventory, CancellationToken cancellationToken)
    {
        var newInventory = MapInventory(rigId, createdDate, inventory);

        var oldInventory = await GetInventoryBySpecification(new InventorySpecification(null, rigId))
                                    .FirstOrDefaultAsync(cancellationToken);

        if (oldInventory != null)
        {
            oldInventory.IsCurrent = false;
            _context.RigInventory.Update(oldInventory);
        }

        await _context.RigInventory.AddAsync(newInventory, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Установить дату и время окончания действия инвентаризации.
    /// </summary>
    /// <param name="rigId"> Идентификатор рига. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    internal async Task SetExpirationDate(Guid rigId, CancellationToken cancellationToken)
    {
        var inventory = await GetInventoryBySpecification(new InventorySpecification(null, rigId))
                                    .FirstOrDefaultAsync(cancellationToken);

        if (inventory != null)
        {
            inventory.EndDateTime = DateTimeOffset.UtcNow;
            _context.RigInventory.Update(inventory);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    /// <summary>
    /// Получить срез инвентаризации за период.
    /// </summary>
    /// <param name="specification"> Спецификация. </param>
    /// <param name="startPeriod"> Начало периода. </param>
    /// <param name="endPeriod"> Конец периода. </param>
    /// <returns> Инвентаризация. </returns>
    private IQueryable<Rigs.RigInventory> GetInventorySliceForAPeriod(InventorySpecification specification,
                                                                              DateTimeOffset startPeriod,
                                                                              DateTimeOffset endPeriod)
    {
        return GetInventoryBySpecification(specification).GetForAPeriod(startPeriod, endPeriod);
    }

    /// <summary>
    /// Получение инвентаризации по спецификации.
    /// </summary>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Инвентаризация. </returns>
    private IQueryable<Rigs.RigInventory> GetInventoryBySpecification(InventorySpecification specification)
    {
        return _context.RigInventory.AsNoTrackingWithIdentityResolution()
                                    .Include(inventory => inventory.Rig)
                                    .Where(inventory => GetRigsBySpecification(specification).Contains(inventory.Rig))
                                    .Actualize(specification);
    }

    private Rigs.RigInventory MapInventory(Guid rigId, DateTimeOffset createdDate, 
                                                   RigInventoryModel inventory)
    {
        return new Rigs.RigInventory()
        {
            RigId = rigId,
            CreatedDateTime = createdDate,
            Cpus = inventory.Cpus,
            Drives = inventory.Drives,
            Gpus = inventory.Gpus,
            NetworkAdapters = inventory.NetworkAdapters,
            Motherboard = inventory.Motherboard,
            Software = inventory.Software
        };
    }
}
