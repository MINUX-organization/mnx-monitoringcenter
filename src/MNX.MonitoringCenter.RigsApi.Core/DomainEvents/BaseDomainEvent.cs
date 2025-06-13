using MediatR;
using MNX.MonitoringCenter.RigsApi.Core.ValueObjects;

namespace MNX.MonitoringCenter.RigsApi.Core.DomainEvents;

/// <summary>
/// Базовое доменное событие.
/// </summary>
/// <param name="RigId"> Идентификатор рига. </param>
public record BaseDomainEvent(RigId RigId) : INotification;
