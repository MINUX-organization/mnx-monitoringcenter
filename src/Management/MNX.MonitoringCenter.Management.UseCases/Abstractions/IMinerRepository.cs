using MNX.MonitoringCenter.Management.Core;

namespace MNX.MonitoringCenter.Management.UseCases.Abstractions;

/// <summary>
/// Репозиторий для доступа к майнерам
/// </summary>
public interface IMinerRepository
{
    /// <summary>
    /// Получить список доступных майнеров
    /// </summary>
    /// <returns> Список доступных майнеров </returns>
    IAsyncEnumerable<Miner> GetAvailableMiners();
}