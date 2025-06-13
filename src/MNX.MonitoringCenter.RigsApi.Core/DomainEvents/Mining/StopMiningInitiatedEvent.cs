using MNX.MonitoringCenter.RigsApi.Core.ValueObjects;

namespace MNX.MonitoringCenter.RigsApi.Core.DomainEvents.Mining;

/// <summary>
/// Событие инициации остановки майнинга.
/// </summary>
/// <param name="RigId"> Идентификатор рига. </param>
/// <param name="InitiatorId"> Идентификатор инициатора. </param>
public record StopMiningInitiatedEvent(RigId RigId, Guid InitiatorId) : BaseDomainEvent(RigId);
