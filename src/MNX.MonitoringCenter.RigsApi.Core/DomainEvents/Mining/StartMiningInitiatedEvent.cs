using MNX.MonitoringCenter.RigsApi.Core.ValueObjects;

namespace MNX.MonitoringCenter.RigsApi.Core.DomainEvents.Mining;

/// <summary>
/// Событие инициации запуска майнинга.
/// </summary>
/// <param name="RigId"> Идентификатор рига. </param>
/// <param name="InitiatorId"> Идентификатор инициатора. </param>
public record StartMiningInitiatedEvent(RigId RigId, Guid InitiatorId) : BaseDomainEvent(RigId);
