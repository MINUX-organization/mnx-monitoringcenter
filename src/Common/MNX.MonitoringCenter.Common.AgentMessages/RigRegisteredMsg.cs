namespace MNX.MonitoringCenter.Common.AgentMessages;

/// <summary>
/// Сообщение о регистрации рига.
/// </summary>
public class RigRegisteredMsg
{
    /// <summary>
    /// Идентификатор рига.
    /// </summary>
    public Guid RigId { get; init; }

    /// <summary>
    /// Идентификатор владельца рига.
    /// </summary>
    public Guid OwnerId { get; init; }
}
