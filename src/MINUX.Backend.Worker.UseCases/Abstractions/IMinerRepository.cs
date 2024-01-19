using MINUX.Backend.Worker.Core;

namespace MINUX.Backend.Worker.UseCases.Abstractions;

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