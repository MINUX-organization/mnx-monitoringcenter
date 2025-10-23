namespace MNX.MonitoringCenter.RigsApi.Contracts.Events;

/// <summary>
/// Интеграционные ивент, обозначающий, что риг сменил имя.
/// </summary>
/// <param name="RigId"> Идентификатор рига. </param>
/// <param name="NewName"> Новое имя рига. </param>
public sealed record RigNameChangedIntegrationEvent(Guid RigId, string NewName);