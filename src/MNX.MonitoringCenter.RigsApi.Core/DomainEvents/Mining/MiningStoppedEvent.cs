using MNX.MonitoringCenter.RigsApi.Core.ValueObjects;

namespace MNX.MonitoringCenter.RigsApi.Core.DomainEvents.Mining;

/// <summary>
/// Событие остановки майнинга.
/// </summary>
/// <param name="RigId"> Идентификатор рига. </param>
public record MiningStoppedEvent(RigId RigId) : BaseDomainEvent(RigId);
