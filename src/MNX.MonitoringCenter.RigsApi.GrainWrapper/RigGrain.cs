using MNX.MonitoringCenter.RigsApi.Core;
using MNX.MonitoringCenter.RigsApi.Core.DomainEvents;
using MNX.MonitoringCenter.RigsApi.Core.LifeCycle;
using MNX.MonitoringCenter.RigsApi.Core.Services;
using MNX.MonitoringCenter.RigsApi.Core.ValueObjects;

namespace MNX.MonitoringCenter.RigsApi.GrainWrapper;

/// <summary>
/// Зерно рига.
/// </summary>
public class RigGrain : IRigGrain
{
    private readonly Rig _rig;

    private readonly IRigRepository _rigRepository;

    /// <inheritdoc/>
    public RigId RigId => _rig.Id;

    /// <inheritdoc/>
    public Guid OwnerId => _rig.OwnerId;

    /// <inheritdoc/>
    public bool IsDecommissioned => _rig.IsDecommissioned;

    ///
    public RigGrain(Rig rig, IRigRepository rigRepository)
    {
        _rig = rig ?? throw new ArgumentNullException(nameof(rig));
        _rigRepository = rigRepository ?? throw new ArgumentNullException(nameof(rigRepository));
    }

    /// <inheritdoc/>
    public Task<RigGrainActionResult> SetInventory(long inventoryId)
        => ChangeAndPersist(() => _rig.SetInventory(inventoryId));

    /// <inheritdoc/>
    public Task<RigGrainActionResult> InitiateTurnOn(Guid initiatorId)
        => ChangeAndPersist(() => _rig.InitiateTurnOn(initiatorId));

    /// <inheritdoc/>
    public Task<RigGrainActionResult> TurnOn()
        => ChangeAndPersist(_rig.TurnOn);

    /// <inheritdoc/>
    public Task<RigGrainActionResult> InitiatePowerOff(Guid initiatorId)
        => ChangeAndPersist(() => _rig.InitiatePowerOff(initiatorId));

    /// <inheritdoc/>
    public Task<RigGrainActionResult> PowerOff()
        => ChangeAndPersist(_rig.PowerOff);

    /// <inheritdoc/>
    public Task<RigGrainActionResult> InitiateReboot(Guid initiatorId)
        => ChangeAndPersist(() => _rig.InitiateReboot(initiatorId));

    /// <inheritdoc/>
    public Task<RigGrainActionResult> InitiateStartMining(Guid initiatorId)
        => ChangeAndPersist(() => _rig.InitiateStartMining(initiatorId));

    /// <inheritdoc/>
    public Task<RigGrainActionResult> TerminateStartMining()
        => ChangeAndPersist(_rig.TerminateStartMining);

    /// <inheritdoc/>
    public Task<RigGrainActionResult> StartMining()
        => ChangeAndPersist(_rig.StartMining);

    /// <inheritdoc/>
    public Task<RigGrainActionResult> InitiateStopMining(Guid initiatorId)
        => ChangeAndPersist(() => _rig.InitiateStopMining(initiatorId));

    /// <inheritdoc/>
    public Task<RigGrainActionResult> TerminateStopMining()
        => ChangeAndPersist(_rig.TerminateStopMining);

    /// <inheritdoc/>
    public Task<RigGrainActionResult> StopMining()
        => ChangeAndPersist(_rig.StopMining);

    /// <inheritdoc/>
    public Task<RigGrainActionResult> Rename(string newName)
        => ChangeAndPersist(() => _rig.Rename(newName));

    /// <inheritdoc/>
    public Task<RigGrainActionResult> Decommission()
        => ChangeAndPersist(_rig.Decommission);
    
    /// <summary>`
    /// Изменить и сохранить.
    /// </summary>
    /// <param name="action"> Действие. </param>
    /// <returns> Результат действия над ригом. </returns>
    private async Task<RigGrainActionResult> ChangeAndPersist(Func<BaseDomainEvent[]> action)
    {
        BaseDomainEvent[]? events;

        try
        {
            events = action();
            await _rigRepository.Update(_rig);
        }
        catch (LifeCycleException ex)
        {
            return RigGrainActionResult.Error(new string[] { ex.Message });
        }

        return RigGrainActionResult.Success(events);
    }
}
