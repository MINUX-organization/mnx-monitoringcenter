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

    /// <summary>
    /// Идентификатор рига.
    /// </summary>
    public RigId RigId { get => _rig.Id; }

    /// <summary>
    /// Идентификатор владельца рига.
    /// </summary>
    public Guid OwnerId { get => _rig.OwnerId; }

    ///
    public RigGrain(Rig rig, IRigRepository rigRepository)
    {
        _rig = rig ?? throw new ArgumentNullException(nameof(rig));
        _rigRepository = rigRepository ?? throw new ArgumentNullException(nameof(rigRepository));
    }

    /// <inheritdoc/>
    public Task<RigGrainActionResult> SetInventory(Guid inventoryId)
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

    /// <summary>
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
