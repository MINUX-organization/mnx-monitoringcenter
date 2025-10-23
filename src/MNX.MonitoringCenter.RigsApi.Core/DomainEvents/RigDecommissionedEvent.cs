using MNX.MonitoringCenter.RigsApi.Core.ValueObjects;

namespace MNX.MonitoringCenter.RigsApi.Core.DomainEvents;

/// <summary>
/// Событие удаления рига.
/// </summary>
/// <param name="RigId"></param>
public sealed record RigDecommissionedEvent(RigId RigId) : BaseDomainEvent(RigId);