using MNX.MonitoringCenter.RigsApi.Core.ValueObjects;

namespace MNX.MonitoringCenter.RigsApi.Core.DomainEvents.Power;

/// <summary>
/// Событие о выключении рига.
/// </summary>
/// <param name="RigId"> Идентификатор рига. </param>
public record RigPoweredOffEvent(RigId RigId) : BaseDomainEvent(RigId);
