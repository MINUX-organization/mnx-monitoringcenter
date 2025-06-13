using MNX.MonitoringCenter.RigsApi.Core.ValueObjects;

namespace MNX.MonitoringCenter.RigsApi.Core.DomainEvents.Power;

/// <summary>
/// Событие инициации перезагрузки рига.
/// </summary>
/// <param name="RigId"> Идентификатор рига. </param>
/// <param name="InitiatorId"> Идентификатор инициатора. </param>
public record RigRebootInitiatedEvent(RigId RigId, Guid InitiatorId) : BaseDomainEvent(RigId);
