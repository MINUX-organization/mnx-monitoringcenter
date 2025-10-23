using MNX.MonitoringCenter.RigsApi.Core.ValueObjects;

namespace MNX.MonitoringCenter.RigsApi.Core.DomainEvents;

/// <summary>
/// События об изменение имени рига.
/// </summary>
/// <param name="RigId"> Идентификатор рига. </param>
/// <param name="NewName"> Новое имя рига. </param>
public record RigNameChangedEvent(RigId RigId, string NewName) : BaseDomainEvent(RigId);
