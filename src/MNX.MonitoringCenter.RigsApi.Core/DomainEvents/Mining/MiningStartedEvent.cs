using MNX.MonitoringCenter.RigsApi.Core.ValueObjects;

namespace MNX.MonitoringCenter.RigsApi.Core.DomainEvents.Mining;

/// <summary>
/// Событие запуска майнинга.
/// </summary>
/// <param name="RigId"> Идентификатор рига. </param>
public record MiningStartedEvent(RigId RigId) : BaseDomainEvent(RigId);
