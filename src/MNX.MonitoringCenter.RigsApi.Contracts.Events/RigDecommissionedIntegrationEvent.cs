namespace MNX.MonitoringCenter.RigsApi.Contracts.Events;

/// <summary>
/// Интеграционный ивент, обозначающий, что риг выведен из эксплуатации.
/// </summary>
/// <param name="RigId"> Идентификатор рига. </param>
public sealed record RigDecommissionedIntegrationEvent(Guid RigId);