namespace MNX.MonitoringCenter.Common.AgentMessages;

/// <summary>
/// Сообщение об отключении рига от сервера.
/// </summary>
/// <param name="RigId"> Идентификатор рига. </param>
public record RigDisconnectedMsg(Guid RigId);
