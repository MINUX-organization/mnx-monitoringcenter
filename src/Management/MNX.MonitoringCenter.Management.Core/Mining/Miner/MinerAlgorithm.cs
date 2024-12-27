namespace MNX.MonitoringCenter.Management.Core.Mining.Miner;

/// <summary>
/// Алгоритм майнера.
/// </summary>
public class MinerAlgorithm
{
    /// <summary>
    /// Идентификатор алгоритма.
    /// </summary>
    public Guid AlgorithmId { get; init; }

    /// <summary>
    /// Идентификатор майнера.
    /// </summary>
    public Guid MinerId { get; init; }

    /// <summary>
    /// Название алгоритма, которое ему присвоил майнер.
    /// </summary>
    public required string Name { get; init; }
}
