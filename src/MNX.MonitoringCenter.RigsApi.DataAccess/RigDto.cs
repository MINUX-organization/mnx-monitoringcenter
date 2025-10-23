using MNX.MonitoringCenter.RigsApi.Core;
using MNX.MonitoringCenter.RigsApi.Core.LifeCycle.Mining;
using MNX.MonitoringCenter.RigsApi.Core.LifeCycle.Power;
using MNX.MonitoringCenter.RigsApi.Core.ValueObjects;

namespace MNX.MonitoringCenter.RigsApi.DataAccess;

/// <summary>
/// Риг.
/// </summary>
public class RigDto
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Идентификатор владельца.
    /// </summary>
    public Guid OwnerId { get; init; }

    /// <summary>
    /// Название.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Идентификатор текущей инвентаризации.
    /// </summary>
    public long? CurrentInventoryId { get; init; }

    /// <summary>
    /// Признак нахождения "В сети".
    /// </summary>
    public bool IsOnline { get; init; }

    /// <summary>
    /// Признак вывода из эксплуатации.
    /// </summary>
    public bool IsDecommissioned { get; private set; }
    
    /// <summary>
    /// Статус жизненного цикла.
    /// </summary>
    public RigLifeCycleStatus LifeCycleStatus { get; init; }

    /// <summary>
    /// Статус жизненного цикла майнинга.
    /// </summary>
    public MiningLifeCycleStatus MiningLifeCycleStatus { get; init; }

    /// <summary>
    /// Маппинг из DTO в доменную сущность.
    /// </summary>
    /// <returns> Сущность рига. </returns>
    public Rig ToDomain() =>
        new Rig(
            id: new RigId(Id),
            ownerId: OwnerId,
            name: Name,
            currentInventoryId: CurrentInventoryId,
            isOnline: IsOnline,
            lifeCycleStatus: LifeCycleStatus,
            miningLifeCycleStatus: MiningLifeCycleStatus,
            isDecommissioned: IsDecommissioned
        );
    
    /// <summary>
    /// Маппинг из доменной сущности в DTO.
    /// </summary>
    /// <param name="rig"> Доменная сущность. </param>
    /// <returns>DTO рига. </returns>
    public static RigDto FromDomain(Rig rig) => 
        new RigDto()
        {
            Id = rig.Id,
            OwnerId = rig.OwnerId,
            Name = rig.Name,
            CurrentInventoryId = rig.CurrentInventoryId,
            IsOnline = rig.IsOnline,
            LifeCycleStatus = rig.LifeCycleStatus,
            MiningLifeCycleStatus = rig.MiningLifeCycleStatus,
            IsDecommissioned = rig.IsDecommissioned
        };
    
}
