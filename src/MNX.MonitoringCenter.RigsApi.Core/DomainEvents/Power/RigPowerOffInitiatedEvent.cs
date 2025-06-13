using MNX.MonitoringCenter.RigsApi.Core.ValueObjects;

namespace MNX.MonitoringCenter.RigsApi.Core.DomainEvents.Power;

/// <summary>
/// Событие инициации выключения рига.
/// </summary>
/// <param name="RigId"> Идентификатор рига. </param>
/// <param name="InitiatorId"> Идентификатор инициатора. </param>
public record RigPowerOffInitiatedEvent(RigId RigId, Guid InitiatorId) : BaseDomainEvent(RigId);
