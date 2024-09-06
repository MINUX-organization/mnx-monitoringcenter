using MNX.MonitoringCenter.Management.Core;

namespace MNX.MonitoringCenter.Management.UseCases.Miner;

/// <summary>
/// Репозиторий для доступа к майнерам
/// </summary>
public interface IMinerRepository
{
    /// <summary>
    /// Получить список доступных майнеров
    /// </summary>
    /// <returns> Список доступных майнеров </returns>
    IAsyncEnumerable<Core.Miner> GetAvailableMiners();
}