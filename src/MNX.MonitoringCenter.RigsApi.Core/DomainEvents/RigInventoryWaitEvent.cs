using MNX.MonitoringCenter.RigsApi.Core.ValueObjects;

namespace MNX.MonitoringCenter.RigsApi.Core.DomainEvents;

/// <summary>
/// Событие ожидания инвентаризации рига.
/// </summary>
/// <param name="RigId"> Идентификатор рига. </param>
public record RigInventoryWaitEvent(RigId RigId) : BaseDomainEvent(RigId);
