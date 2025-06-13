using MNX.MonitoringCenter.RigsApi.Core.DomainEvents;
using MNX.MonitoringCenter.RigsApi.Core.DomainEvents.Mining;
using MNX.MonitoringCenter.RigsApi.Core.DomainEvents.Power;
using MNX.MonitoringCenter.RigsApi.Core.LifeCycle.Mining;
using MNX.MonitoringCenter.RigsApi.Core.LifeCycle.Power;
using MNX.MonitoringCenter.RigsApi.Core.ValueObjects;

namespace MNX.MonitoringCenter.RigsApi.Core;

/// <summary>
/// Риг.
/// </summary>
public class Rig
{
    private RigLifeCycleStateMachine _lifeCycleStateMachine;

    private MiningLifeCycleStateMachine _miningStateMachine;

    /// <summary>
    /// Идентификатор.
    /// </summary>
    public RigId Id { get; init; }
    
    /// <summary>
    /// Идентификатор владельца.
    /// </summary>
    public Guid OwnerId { get; set; }

    /// <summary>
    /// Название.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Идентификатор текущей инвентаризации.
    /// </summary>
    public Guid? CurrentInventoryId { get; private set; }

    /// <summary>
    /// Признак нахождения "В сети".
    /// </summary>
    public bool IsOnline { get; private set; }

    /// <summary>
    /// Статус жизненного цикла.
    /// </summary>
    public RigLifeCycleStatus LifeCycleStatus { get => _lifeCycleStateMachine.Status; }

    /// <summary>
    /// Статус жизненного цикла майнинга.
    /// </summary>
    public MiningLifeCycleStatus MiningLifeCycleStatus { get => _miningStateMachine.Status; }

    public Rig(RigId id, Guid ownerId, string name)
    {
        Id = id;
        OwnerId = ownerId;
        Name = name;
        CurrentInventoryId = Guid.Empty;
        IsOnline = false;

        _lifeCycleStateMachine = RigLifeCycleStateMachine.CreateStateMachine(RigLifeCycleStatus.Disable);
        _miningStateMachine = MiningLifeCycleStateMachine.CreateStateMachine(MiningLifeCycleStatus.Disable);
    }

    public Rig(RigId id, Guid ownerId, string name, Guid? currentInventoryId, bool isOnline,
               RigLifeCycleStatus lifeCycleStatus, MiningLifeCycleStatus miningLifeCycleStatus)
    {
        Id = id;
        OwnerId = ownerId;
        Name = name;
        CurrentInventoryId = currentInventoryId;
        IsOnline = isOnline;

        _lifeCycleStateMachine = RigLifeCycleStateMachine.CreateStateMachine(lifeCycleStatus);
        _miningStateMachine = MiningLifeCycleStateMachine.CreateStateMachine(miningLifeCycleStatus);
    }

    /// <summary>
    /// Задать инвентаризацию.
    /// </summary>
    /// <param name="inventoryId"> Идентификатор инвентаризации. </param>
    /// <returns> Доменные события. </returns>
    public BaseDomainEvent[] SetInventory(Guid inventoryId)
    {
        if (inventoryId == Guid.Empty)
            throw new ArgumentException("Inventory id couldn't be empty", nameof(inventoryId));

        CurrentInventoryId = inventoryId;
        return Array.Empty<BaseDomainEvent>();
    }

    #region power
    /// <summary>
    /// Инициировать включение.
    /// </summary>
    /// <param name="initiatorId"> Идентификатор инициатора. </param>
    /// <returns> Доменные события. </returns>
    public BaseDomainEvent[] InitiateTurnOn(Guid initiatorId)
    {
        _lifeCycleStateMachine = _lifeCycleStateMachine.InitiateTurnOn();
        return new BaseDomainEvent[] { new RigTurnOnInitiatedEvent(Id, initiatorId) };
    }

    /// <summary>
    /// Включить.
    /// </summary>
    /// <returns> Доменные события. </returns>
    public BaseDomainEvent[] TurnOn()
    {
        _lifeCycleStateMachine = _lifeCycleStateMachine.TurnOn();
        IsOnline = true;
        CurrentInventoryId = null;
        return new BaseDomainEvent[] { new RigTurnedOnEvent(Id), new RigInventoryWaitEvent(Id) };
    }

    /// <summary>
    /// Инициировать выключение.
    /// </summary>
    /// <param name="initiatorId"> Идентификатор инициатора. </param>
    /// <returns> Доменные события. </returns>
    public BaseDomainEvent[] InitiatePowerOff(Guid initiatorId)
    {
        _lifeCycleStateMachine = _lifeCycleStateMachine.InitiatePowerOff();
        return new BaseDomainEvent[] { new RigPowerOffInitiatedEvent(Id, initiatorId) };
    }

    /// <summary>
    /// Выключить.
    /// </summary>
    /// <returns> Доменные события. </returns>
    public BaseDomainEvent[] PowerOff()
    {
        _lifeCycleStateMachine = _lifeCycleStateMachine.PowerOff();
        _miningStateMachine = _miningStateMachine.Stop();
        IsOnline = false;

        return new BaseDomainEvent[] { new RigPoweredOffEvent(Id) };
    }

    /// <summary>
    /// Инициировать перезагрузку.
    /// </summary>
    /// <param name="initiatorId"> Идентификатор инициатора. </param>
    /// <returns> Доменные события. </returns>
    public BaseDomainEvent[] InitiateReboot(Guid initiatorId)
    {
        _lifeCycleStateMachine = _lifeCycleStateMachine.InitiatePowerOff();
        return new BaseDomainEvent[] { new RigRebootInitiatedEvent(Id, initiatorId) };
    }
    #endregion

    #region mining
    /// <summary>
    /// Инициировать запуск майнинга.
    /// </summary>
    /// <param name="initiatorId"> Идентификатор инициатора. </param>
    /// <returns> Доменные события. </returns>
    public BaseDomainEvent[] InitiateStartMining(Guid initiatorId)
    {
        _miningStateMachine = _miningStateMachine.InitiateStart();
        return new BaseDomainEvent[] { new StartMiningInitiatedEvent(Id, initiatorId) };
    }

    /// <summary>
    /// Прервать запуск майнинга.
    /// </summary>
    /// <returns> Доменные события. </returns>
    public BaseDomainEvent[] TerminateStartMining()
    {
        _miningStateMachine = _miningStateMachine.TerminateStart();
        return Array.Empty<BaseDomainEvent>();
    }

    /// <summary>
    /// Запустить майнинг.
    /// </summary>
    /// <returns> Доменные события. </returns>
    public BaseDomainEvent[] StartMining()
    {
        _miningStateMachine = _miningStateMachine.Start();
        return new BaseDomainEvent[] { new MiningStartedEvent(Id) };
    }

    /// <summary>
    /// Инициировать остановку майнинга.
    /// </summary>
    /// <param name="initiatorId"> Идентификатор инициатора. </param>
    /// <returns> Доменные события. </returns>
    public BaseDomainEvent[] InitiateStopMining(Guid initiatorId)
    {
        _miningStateMachine = _miningStateMachine.InitiateStop();
        return new BaseDomainEvent[] { new StopMiningInitiatedEvent(Id, initiatorId) };
    }

    /// <summary>
    /// Прервать остановку майнинга.
    /// </summary>
    /// <returns> Доменные события. </returns>
    public BaseDomainEvent[] TerminateStopMining()
    {
        _miningStateMachine = _miningStateMachine.TerminateStop();
        return Array.Empty<BaseDomainEvent>();
    }

    /// <summary>
    /// Остановить майнинг.
    /// </summary>
    /// <returns> Доменные события. </returns>
    public BaseDomainEvent[] StopMining()
    {
        _miningStateMachine = _miningStateMachine.InitiateStop();
        return new BaseDomainEvent[] { new MiningStoppedEvent(Id) };
    }
    #endregion
}
