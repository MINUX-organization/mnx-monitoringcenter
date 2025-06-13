using MNX.MonitoringCenter.RigsApi.Core.ValueObjects;

namespace MNX.MonitoringCenter.RigsApi.Core.DomainEvents.Power;

/// <summary>
/// Событие включения рига.
/// </summary>
/// <param name="RigId"> Идентификатор рига. </param>
public record RigTurnedOnEvent(RigId RigId) : BaseDomainEvent(RigId);
