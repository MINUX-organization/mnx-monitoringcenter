using MNX.MonitoringCenter.RigsApi.Core.DomainEvents;
using MNX.MonitoringCenter.RigsApi.Core.DomainEvents.Mining;
using MNX.MonitoringCenter.RigsApi.Core.DomainEvents.Power;
using MNX.MonitoringCenter.RigsApi.Core.LifeCycle;
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
    public long? CurrentInventoryId { get; private set; }

    /// <summary>
    /// Признак нахождения "В сети".
    /// </summary>
    public bool IsOnline { get; private set; }
    
    /// <summary>
    /// Признак вывода из эксплуатации.
    /// </summary>
    public bool IsDecommissioned { get; private set; }

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
        CurrentInventoryId = null;
        IsOnline = false;
        IsDecommissioned = false;
        
        _lifeCycleStateMachine = RigLifeCycleStateMachine.CreateStateMachine(RigLifeCycleStatus.Disable);
        _miningStateMachine = MiningLifeCycleStateMachine.CreateStateMachine(MiningLifeCycleStatus.Disable);
    }

    public Rig(RigId id, Guid ownerId, string name, long? currentInventoryId, bool isOnline,
               RigLifeCycleStatus lifeCycleStatus, MiningLifeCycleStatus miningLifeCycleStatus, bool isDecommissioned)
    {
        Id = id;
        OwnerId = ownerId;
        Name = name;
        CurrentInventoryId = currentInventoryId;
        IsOnline = isOnline;
        IsDecommissioned = isDecommissioned;
        
        _lifeCycleStateMachine = RigLifeCycleStateMachine.CreateStateMachine(lifeCycleStatus);
        _miningStateMachine = MiningLifeCycleStateMachine.CreateStateMachine(miningLifeCycleStatus);
    }

    /// <summary>
    /// Задать инвентаризацию.
    /// </summary>
    /// <param name="inventoryId"> Идентификатор инвентаризации. </param>
    /// <returns> Доменные события. </returns>
    public BaseDomainEvent[] SetInventory(long? inventoryId) => ExecuteOperation((() =>
    {
        if (inventoryId is null or <= 0)
            throw new ArgumentException("Inventory id couldn't be null or negative", nameof(inventoryId));

        CurrentInventoryId = inventoryId;
        return Array.Empty<BaseDomainEvent>();
    }));

    #region power

    /// <summary>
    /// Инициировать включение.
    /// </summary>
    /// <param name="initiatorId"> Идентификатор инициатора. </param>
    /// <returns> Доменные события. </returns>
    public BaseDomainEvent[] InitiateTurnOn(Guid initiatorId) => ExecuteOperation((() =>
    {
        _lifeCycleStateMachine = _lifeCycleStateMachine.InitiateTurnOn();
        return new BaseDomainEvent[] { new RigTurnOnInitiatedEvent(Id, initiatorId) };
    }));

    /// <summary>
    /// Включить.
    /// </summary>
    /// <returns> Доменные события. </returns>
    public BaseDomainEvent[] TurnOn() => ExecuteOperation((() =>
    {
        _lifeCycleStateMachine = _lifeCycleStateMachine.TurnOn();
        IsOnline = true;
        CurrentInventoryId = null;
        return new BaseDomainEvent[] { new RigTurnedOnEvent(Id), new RigInventoryWaitEvent(Id) };
    }));

    /// <summary>
    /// Инициировать выключение.
    /// </summary>
    /// <param name="initiatorId"> Идентификатор инициатора. </param>
    /// <returns> Доменные события. </returns>
    public BaseDomainEvent[] InitiatePowerOff(Guid initiatorId) => ExecuteOperation((() =>
    {
        _lifeCycleStateMachine = _lifeCycleStateMachine.InitiatePowerOff();
        return new BaseDomainEvent[] { new RigPowerOffInitiatedEvent(Id, initiatorId) };
    }));

    /// <summary>
    /// Выключить.
    /// </summary>
    /// <returns> Доменные события. </returns>
    public BaseDomainEvent[] PowerOff() => ExecuteOperation((() =>
    {
        _lifeCycleStateMachine = _lifeCycleStateMachine.PowerOff();
        _miningStateMachine = _miningStateMachine.EnsureStopping();
        IsOnline = false;

        return new BaseDomainEvent[] { new RigPoweredOffEvent(Id) };
    }));

    /// <summary>
    /// Инициировать перезагрузку.
    /// </summary>
    /// <param name="initiatorId"> Идентификатор инициатора. </param>
    /// <returns> Доменные события. </returns>
    public BaseDomainEvent[] InitiateReboot(Guid initiatorId) => ExecuteOperation((() =>
    {
        _lifeCycleStateMachine = _lifeCycleStateMachine.InitiatePowerOff();
        return new BaseDomainEvent[] { new RigRebootInitiatedEvent(Id, initiatorId) };
    }));
    
    #endregion

    #region mining

    /// <summary>
    /// Инициировать запуск майнинга.
    /// </summary>
    /// <param name="initiatorId"> Идентификатор инициатора. </param>
    /// <returns> Доменные события. </returns>
    public BaseDomainEvent[] InitiateStartMining(Guid initiatorId) => ExecuteOperation((() =>
    {
        _miningStateMachine = _miningStateMachine.InitiateStart();
        return new BaseDomainEvent[] { new StartMiningInitiatedEvent(Id, initiatorId) };
    }));

    /// <summary>
    /// Прервать запуск майнинга.
    /// </summary>
    /// <returns> Доменные события. </returns>
    public BaseDomainEvent[] TerminateStartMining() => ExecuteOperation((() =>
    {
        _miningStateMachine = _miningStateMachine.TerminateStart();
        return Array.Empty<BaseDomainEvent>();
    }));

    /// <summary>
    /// Запустить майнинг.
    /// </summary>
    /// <returns> Доменные события. </returns>
    public BaseDomainEvent[] StartMining() => ExecuteOperation((() =>
    {
        _miningStateMachine = _miningStateMachine.Start();
        return new BaseDomainEvent[] { new MiningStartedEvent(Id) };
    }));

    /// <summary>
    /// Инициировать остановку майнинга.
    /// </summary>
    /// <param name="initiatorId"> Идентификатор инициатора. </param>
    /// <returns> Доменные события. </returns>
    public BaseDomainEvent[] InitiateStopMining(Guid initiatorId) => ExecuteOperation((() =>
    {
        _miningStateMachine = _miningStateMachine.InitiateStop();
        return new BaseDomainEvent[] { new StopMiningInitiatedEvent(Id, initiatorId) };
    }));

    /// <summary>
    /// Прервать остановку майнинга.
    /// </summary>
    /// <returns> Доменные события. </returns>
    public BaseDomainEvent[] TerminateStopMining() => ExecuteOperation((() =>
    {
        _miningStateMachine = _miningStateMachine.TerminateStop();
        return Array.Empty<BaseDomainEvent>();
    }));

    /// <summary>
    /// Остановить майнинг.
    /// </summary>
    /// <returns> Доменные события. </returns>
    public BaseDomainEvent[] StopMining() => ExecuteOperation((() =>
    {
        _miningStateMachine = _miningStateMachine.Stop();
        return new BaseDomainEvent[] { new MiningStoppedEvent(Id) };
    }));
    #endregion

    /// <summary>
    /// Переименовать риг.
    /// </summary>
    /// <param name="newName"> Новое имя для рига. </param>
    /// <returns> Доменные события. </returns>
    public BaseDomainEvent[] Rename(string newName) => ExecuteOperation((() =>
    {
        Name = newName;
        return new BaseDomainEvent[] { new RigNameChangedEvent(Id,  newName) };
    }));
    
    /// <summary>
    /// Вывести риг из эксплуатации.
    /// </summary>
    /// <returns> Доменные события. </returns>
    public BaseDomainEvent[] Decommission()
    {
        if (IsDecommissioned || IsOnline)
            throw new LifeCycleException(
                message: "Невозможно вывести риг из строя.",
                currentStatus: $"{MiningLifeCycleStatus}, {LifeCycleStatus}"
            );
        
        IsDecommissioned = true;
        return new BaseDomainEvent[] { new RigDecommissionedEvent(Id) };
    }

    /// <summary>
    /// Выполняет доменную операцию, предварительно убедившись, что агрегат не выведен из эксплуатации.
    /// </summary>
    /// <param name="operation"> Доменная функция, содержащая логику изменения состояния и возвращающая результат. </param>
    /// <typeparam name="T"> Тип результата, возвращаемого операцией. </typeparam>
    /// <returns> Результат выполнения доменной функции.  </returns>
    /// <exception cref="LifeCycleException"> Бросается, если риг находится в состоянии IsDecommissioned. </exception>
    private T ExecuteOperation<T>(Func<T> operation)
    {
        if (IsDecommissioned)
            throw new LifeCycleException("Риг выведен из строя. Любые операции не доступны.", "Выведен из строя");

        return operation();
    }
}
